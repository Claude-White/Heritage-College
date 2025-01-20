using CWhiteH60Services.DAL;
using CWhiteH60Services.Models;
using Microsoft.EntityFrameworkCore;

namespace CWhiteH60ServicesTest;

public class CRUDCartItemTest : IDisposable {
    private readonly H60AssignmentDbCWContext _context;
    private readonly ICartItemRepository<CartItem> _cartItemRepository;

    public CRUDCartItemTest() {
        var options = new DbContextOptionsBuilder<H60AssignmentDbCWContext>()
            .UseInMemoryDatabase(databaseName: new Guid().ToString())
            .Options;

        _context = new H60AssignmentDbCWContext(options);
        _context.Database.EnsureCreated();
        
        _context.ProductCategories.AddRange(
            new ProductCategory { CategoryId = 100, ProdCat = "Foil Boards" },
            new ProductCategory { CategoryId = 200, ProdCat = "Front Wings" },
            new ProductCategory { CategoryId = 300, ProdCat = "Stabilizers" },
            new ProductCategory { CategoryId = 400, ProdCat = "Fuselage" },
            new ProductCategory { CategoryId = 500, ProdCat = "Masts" }
        );
        
        _context.Products.AddRange(
            new Product { 
                ProductId = 100, 
                ProdCatId = 200, 
                Description = "Sonar MA1850v2", 
                Manufacturer = "North", 
                Stock = 4, 
                BuyPrice = 750.00m, 
                SellPrice = 900.00m },
            new Product { 
                ProductId = 200, 
                ProdCatId = 200, 
                Description = "G 100 Front Wing V1", 
                Manufacturer = "Slingshot", 
                Stock = 8, 
                BuyPrice = 550.00m, 
                SellPrice = 630.00m },
            new Product {
                ProductId = 700, 
                ProdCatId = 400, 
                Description = "Black Standard Fuselage", 
                Manufacturer = "Axis", 
                Stock = 34, 
                BuyPrice = 300.00m, 
                SellPrice = 350.00m
            },
            new Product {
                ProductId = 1400, 
                ProdCatId = 300, 
                Description = "Phantasm Stabilizer 340 Turbo-Tail V1", 
                Manufacturer = "Slingshot", 
                Stock = 46, 
                BuyPrice = 195.00m, 
                SellPrice = 230.00m
            },
            new Product {
                ProductId = 1900, 
                ProdCatId = 500, 
                Description = "CARBON MAST 16", 
                Manufacturer = "F-ONE", 
                Stock = 5, 
                BuyPrice = 380.00m, 
                SellPrice = 470.00m
            }
        );

        _context.Customers.AddRange(
            new Customer {
                CustomerId = 100, 
                FirstName = "Claude", 
                LastName = "White", 
                Email = "claude@white.com",
                PhoneNumber = "1231231234", 
                Province = "QC", 
                CreditCard = "1234123412341234",
                UserId = ""
            },
            new Customer {
                CustomerId = 200, 
                FirstName = "Ryan", 
                LastName = "Somers", 
                Email = "ryan@somers.com",
                PhoneNumber = "1231231234", 
                Province = "QC", 
                CreditCard = "1234123412341234",
                UserId = ""
            },
            new Customer {
                CustomerId = 300, 
                FirstName = "Pierre", 
                LastName = "Badra", 
                Email = "pierre@badra.com",
                PhoneNumber = "1231231234", 
                Province = "QC", 
                CreditCard = "1234123412341234",
                UserId = ""
            },
            new Customer {
                CustomerId = 400, 
                FirstName = "Matteo", 
                LastName = "Falasconi", 
                Email = "matteo@falasconi.com",
                PhoneNumber = "1231231234", 
                Province = "QC", 
                CreditCard = "1234123412341234",
                UserId = ""
            }
        );
        
        _context.ShoppingCarts.AddRange(
            new ShoppingCart {
                CartId = 100, 
                CustomerId = 100, 
                DateCreated = DateTime.Now
            },
            new ShoppingCart {
                CartId = 200, 
                CustomerId = 200, 
                DateCreated = DateTime.Now
            }
        );
        
        _context.CartItems.AddRange(
            new CartItem {
                CartItemId = 100, 
                CartId = 100, 
                ProductId = 1900, 
                Quantity = 1
            },
            new CartItem {
                CartItemId = 200, 
                CartId = 100, 
                ProductId = 700, 
                Quantity = 3
            },
            new CartItem 
            {
                CartItemId = 300, 
                CartId = 100, 
                ProductId = 1400, 
                Quantity = 1
            },
            new CartItem 
            {
                CartItemId = 400, 
                CartId = 200, 
                ProductId = 200,
                Quantity = 2
            }
        );
        
        _context.SaveChanges();

        _cartItemRepository = new CartItemRepository(_context);
    }

    public void Dispose()
    {
        _context.CartItems.RemoveRange(_context.CartItems);
        _context.ShoppingCarts.RemoveRange(_context.ShoppingCarts);
        _context.Customers.RemoveRange(_context.Customers);
        _context.Products.RemoveRange(_context.Products);
        _context.ProductCategories.RemoveRange(_context.ProductCategories);
        
        _context.SaveChanges();
        
        _context.Dispose();
    }
    
    [Fact]
    public async void Test_Create_CartItem() {
        // Arrange
        var cartItem = new CartItem {
            CartItemId = 500,
            CartId = 200,
            ProductId = 200,
            Quantity = 3
        };
        // Act
        Assert.True(await _cartItemRepository.Create(cartItem));
        // Assert
        var customerShoppingCart = _context
            .Customers
            .Include(c => c.ShoppingCart)
            .ThenInclude(shoppingCart => shoppingCart!.CartItems)
            .First(c => c.ShoppingCart!.CartId == 200)
            .ShoppingCart;
        Assert.NotNull(customerShoppingCart!
            .CartItems
            .FirstOrDefault(ci => ci.CartItemId == 500));
    }
    
    [Fact]
    public async void Test_Read_CartItem() {
        // Arrange
        // In Constructor
        // Act
        var cartItems = await _cartItemRepository.Read();
        // Assert
        Assert.Equal(4, cartItems.Count);
        Assert.Equal(100, cartItems
            .First(sc => sc.CartItemId == 100)
            .Cart.CartId);
        Assert.Equal(200, cartItems
            .First(sc => sc.CartItemId == 400)
            .Cart.CartId);
    }
    
    [Fact]
    public async void Test_Update_CartItem() {
        // Arrange
        var cartItem = _context
            .CartItems
            .Include(ci => ci.Cart)
            .First(ci => ci.CartItemId == 100);
        // Assert
        Assert.NotNull(cartItem);
        Assert.Equal(100, cartItem.Cart.CartId);
        // Act
        cartItem.Quantity = 7;
        Assert.True(await _cartItemRepository.Update(cartItem));
        // Assert
        Assert.Equal(7, _context
            .CartItems
            .First(ci => ci.CartItemId == 100)
            .Quantity);
    }

    [Fact]
    public async void Test_Delete_CartItem() {
        // Arrange
        var cartItem = _context
            .CartItems
            .Include(ci => ci.Cart)
            .First(ci => ci.CartItemId == 100);
        var shoppingCart = cartItem.Cart;
        // Assert
        Assert.NotNull(cartItem);
        Assert.NotNull(shoppingCart);
        // Act
        Assert.True(await _cartItemRepository.Delete(cartItem));
        // Assert
        Assert.Null(_context
            .CartItems
            .FirstOrDefault(ci => ci.CartItemId == 100));
        Assert.NotNull(_context
            .ShoppingCarts
            .FirstOrDefault(sc => sc.CartId == 100));
    }
    
    [Fact]
    public async void Test_Find_CartItem() {
        // Arrange
        // In Constructor
        // Act
        var foundCartItem = await _cartItemRepository.Find(100);
        // Assert
        Assert.NotNull(foundCartItem);
        Assert.Equal(1, _context
            .CartItems
            .First(ci => ci.CartItemId == 100)
            .Quantity);
        Assert.Equal(1900, _context
            .CartItems
            .First(ci => ci.CartItemId == 100)
            .ProductId);
    }
}