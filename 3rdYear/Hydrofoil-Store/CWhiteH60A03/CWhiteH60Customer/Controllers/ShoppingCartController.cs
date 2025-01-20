using AspNetCoreHero.ToastNotification.Abstractions;
using CWhiteH60Customer.DAL;
using CWhiteH60Customer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CWhiteH60Customer.Controllers;

public class ShoppingCartController : Controller {
    private readonly IShoppingCartRepository<ShoppingCart> _shoppingCartRepository;
    private readonly ICartItemRepository<CartItem> _cartItemRepository;
    private readonly IProductRepository<Product> _productRepository;
    private readonly IOrderRepository<Order> _orderRepository;
    private readonly IOrderItemRepository<OrderItem> _orderItemRepository;
    private readonly INotyfService _notifyService;

    public ShoppingCartController(IShoppingCartRepository<ShoppingCart> shoppingCartRepository,
        ICartItemRepository<CartItem> cartItemRepository,
        IProductRepository<Product> productRepository,
        IOrderRepository<Order> orderRepository,
        IOrderItemRepository<OrderItem> orderItemRepository,
        INotyfService notifyService) {
        _shoppingCartRepository = shoppingCartRepository;
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _notifyService = notifyService;
    }
    
    [HttpGet]
    public async Task<ActionResult> ShoppingCart(int id) {
        var shoppingCart = await _shoppingCartRepository.GetByCustomerIdInclude(id);
        return View(shoppingCart);
    }

    [HttpPost("AddToCart")]
    public async Task<ActionResult> AddToCart([FromForm] int customerId, int productId, int quantity) {
        var product = await _productRepository.GetById(productId);
        var cart = await _shoppingCartRepository.GetByCustomerIdInclude(customerId);
        var sameItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

        if (sameItem != null) {
            sameItem.Quantity += quantity;
            sameItem.Product = null;
            sameItem.Cart = null;
            await _cartItemRepository.Update(sameItem);
        }
        else {
            var cartItem = new CartItem {
                CartId = cart.CartId,
                ProductId = product.ProductId,
                Quantity = quantity,
                Price = product.SellPrice
            };
            var success = await _cartItemRepository.Create(cartItem);
            if (!success) {
                return BadRequest();
            }
        }
        
        return Ok();
    }

    [HttpGet("RemoveFromCart/{id:int}")]
    public async Task<ActionResult> RemoveFromCart(int id) {
        var cartItem = await _cartItemRepository.GetById(id);
        var success = await _cartItemRepository.Delete(cartItem);
        if (!success) {
            return BadRequest();
        }
        return Ok();
    }
    
    public async Task<IActionResult> PlaceOrder(Customer customer) {
        ModelState.Remove("UserId");
        ModelState.Remove("Email");
        
        var shoppingCart = await _shoppingCartRepository.GetByCustomerIdInclude(customer.CustomerId);
        
        if (!ModelState.IsValid) {
            return View("ShoppingCart", shoppingCart);
        }
        
        var subTotal = shoppingCart.CartItems.Sum(ci => ci.Price * ci.Quantity);

        var taxes = 0m;

        using (var client = new HttpClient()) {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5115";
            client.BaseAddress = new Uri($"{baseUrl}");
            
            var response = await client.GetAsync($"/api/Checkout/CalculateTax/{subTotal}/{customer.Province}");
            if (!response.IsSuccessStatusCode) {
                ModelState.AddModelError(string.Empty, "Unable to calculate tax");
                return View("ShoppingCart", shoppingCart);
            }
            var responseString = await response.Content.ReadAsStringAsync();
            taxes = decimal.TryParse(responseString, out var result) ? result : 0m;
        }

        var now = DateTime.Now;
        var newOrder = new Order {
            CustomerId = customer.CustomerId,
            DateCreated = now,
            Total = subTotal,
            Taxes = taxes
        };
        
        await _orderRepository.Create(newOrder);
        
        var customerOrders = await _orderRepository.GetByCustomerId(customer.CustomerId);
        var order = customerOrders.FirstOrDefault(o => o.DateCreated.Date == now.Date);

        if (order == null) {
            return View("ShoppingCart", shoppingCart);
        }
        
        foreach (var item in shoppingCart.CartItems) {
            var orderItem = new OrderItem {
                OrderId = order.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price,
            };
            await _orderItemRepository.Create(orderItem);
        }
        
        _notifyService.Success("Order successfully placed");
        return RedirectToAction("ShoppingCart", "ShoppingCart", new { id = customer.CustomerId });
    }
}