using CWhiteH60Services.DAL;
using CWhiteH60Services.Models;
using CWhiteH60Services.Models.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace CWhiteH60Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartItemController : ControllerBase {
    private readonly ICartItemRepository<CartItem> _cartItemRepository;
    private readonly IStoreRepository<Product> _storeRepository;

    public CartItemController(ICartItemRepository<CartItem> cartItemRepository, IStoreRepository<Product> storeRepository) {
        _cartItemRepository = cartItemRepository;
        _storeRepository = storeRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<CartItem>>> AllCartItems() {
        return await _cartItemRepository.Read();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CartItem>> FindCartItem(int id) {
        var cartItem = await _cartItemRepository.Find(id);
        if (cartItem == null) {
            return NotFound();
        }
        return cartItem;
    }

    [HttpPost]
    public async Task<ActionResult> CreateCart(CartItemDto cartItemDto) {
        var cartItem = new CartItem(cartItemDto);
        var success = await _cartItemRepository.Create(cartItem);
        if (!success) {
            return BadRequest(ModelState);
        }
        return CreatedAtAction(nameof(CreateCart), new { id = cartItem.CartItemId }, cartItem);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCart(CartItemDto cartItemDto) {
        var cartItem = new CartItem(cartItemDto);
        var success = await _cartItemRepository.Update(cartItem);
        if (!success) {
            return BadRequest(ModelState);
        }
        return Ok(cartItem);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCart(int id) {
        var cartItem = await _cartItemRepository.Find(id);
        if (cartItem == null) {
            return NotFound();
        }
        
        var success = await _cartItemRepository.Delete(cartItem);
        if (!success) {
            return BadRequest(ModelState);
        }
        return NoContent();
    }

    [HttpPatch("BatchUpdateQuantity")]
    public async Task<IActionResult> BatchUpdateQuantity([FromBody] List<CartItemUpdateDto> updates) {
        if (updates == null || !updates.Any()) {
            return BadRequest("No updates provided");
        }

        try {
            foreach (var update in updates) {
                var cartItem = await _cartItemRepository.Find(update.ItemId);
                if (cartItem == null) {
                    return NotFound($"Cart item {update.ItemId} not found");
                }
                
                cartItem.Quantity += update.QuantityChange;
                
                var product = await _storeRepository.GetById(cartItem.ProductId);
                product.Stock -= update.QuantityChange;

                if (product.Stock < 0) {
                    return Ok($"Product ({product.Description}) out of stock");
                }
                
                if (cartItem.Quantity <= 0) {
                    await _cartItemRepository.Delete(cartItem);
                }
                
                await _cartItemRepository.Update(cartItem);
            }
            
            return Ok();
        }
        catch (Exception ex) {
            return StatusCode(500, "An error occurred while updating cart items");
        }
    }
}