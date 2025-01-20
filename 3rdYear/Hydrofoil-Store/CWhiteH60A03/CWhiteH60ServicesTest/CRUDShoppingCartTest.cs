using CWhiteH60Services.DAL;
using CWhiteH60Services.Models;
using Microsoft.EntityFrameworkCore;

namespace CWhiteH60ServicesTest;

public class CRUDShoppingCartTest : IDisposable {
    
    private readonly H60AssignmentDbCWContext _context;
    private readonly IShoppingCartRepository<ShoppingCart> _shoppingCartRepository;

    public CRUDShoppingCartTest() {
        var options = new DbContextOptionsBuilder<H60AssignmentDbCWContext>()
            .UseInMemoryDatabase(databaseName: new Guid().ToString())
            .Options;

        _context = new H60AssignmentDbCWContext(options);
        _context.Database.EnsureCreated();
        
        _context.ProductCategories.AddRange(
            new ProductCategory { CategoryId = 1000, ProdCat = "Foil Boards" },
            new ProductCategory { CategoryId = 2000, ProdCat = "Front Wings" },
            new ProductCategory { CategoryId = 3000, ProdCat = "Stabilizers" },
            new ProductCategory { CategoryId = 4000, ProdCat = "Fuselage" },
            new ProductCategory { CategoryId = 5000, ProdCat = "Masts" }
        );
        
        _context.Products.AddRange(
            new Product { 
                ProductId = 1000, 
                ProdCatId = 2000, 
                Description = "Sonar MA1850v2", 
                Manufacturer = "North", 
                Stock = 4, 
                BuyPrice = 750.00m, 
                SellPrice = 900.00m },
            new Product { 
                ProductId = 2000, 
                ProdCatId = 2000, 
                Description = "G 1000 Front Wing V1", 
                Manufacturer = "Slingshot", 
                Stock = 8, 
                BuyPrice = 550.00m, 
                SellPrice = 630.00m },
            new Product {
                ProductId = 7000, 
                ProdCatId = 4000, 
                Description = "Black Standard Fuselage", 
                Manufacturer = "Axis", 
                Stock = 34, 
                BuyPrice = 300.00m, 
                SellPrice = 350.00m
            },
            new Product {
                ProductId = 14000, 
                ProdCatId = 3000, 
                Description = "Phantasm Stabilizer 340 Turbo-Tail V1", 
                Manufacturer = "Slingshot", 
                Stock = 46, 
                BuyPrice = 195.00m, 
                SellPrice = 230.00m
            },
            new Product {
                ProductId = 19000, 
                ProdCatId = 5000, 
                Description = "CARBON MAST 16", 
                Manufacturer = "F-ONE", 
                Stock = 5, 
                BuyPrice = 380.00m, 
                SellPrice = 470.00m
            }
        );

        _context.Customers.AddRange(
            new Customer {
                CustomerId = 1000, 
                FirstName = "Claude", 
                LastName = "White", 
                Email = "claude@white.com",
                PhoneNumber = "1231231234", 
                Province = "QC", 
                CreditCard = "1234123412341234",
                UserId = ""
            },
            new Customer {
                CustomerId = 2000, 
                FirstName = "Ryan", 
                LastName = "Somers", 
                Email = "ryan@somers.com",
                PhoneNumber = "1231231234", 
                Province = "QC", 
                CreditCard = "1234123412341234",
                UserId = ""
            },
            new Customer {
                CustomerId = 3000, 
                FirstName = "Pierre", 
                LastName = "Badra", 
                Email = "pierre@badra.com",
                PhoneNumber = "1231231234", 
                Province = "QC", 
                CreditCard = "1234123412341234",
                UserId = ""
            },
            new Customer {
                CustomerId = 4000, 
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
                CartId = 1000, 
                CustomerId = 1000, 
                DateCreated = DateTime.Now
            },
            new ShoppingCart {
                CartId = 2000, 
                CustomerId = 2000, 
                DateCreated = DateTime.Now
            }
        );
        
        _context.CartItems.AddRange(
            new CartItem { CartId = 1000, ProductId = 19000, Quantity = 1 },
            new CartItem { CartId = 1000, ProductId = 7000, Quantity = 3 },
            new CartItem { CartId = 1000, ProductId = 14000, Quantity = 1 },
            new CartItem { CartId = 2000, ProductId = 2000, Quantity = 2 }
        );
        
        _context.SaveChanges();

        _shoppingCartRepository = new ShoppingCartRepository(_context);
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
    public async void Test_Create_ShoppingCart() {
        // Arrange
        var shoppingCart = new ShoppingCart {
            CartId = 3000, 
            CustomerId = 3000, 
            DateCreated = DateTime.Now
        };
        // Act
        Assert.True(await _shoppingCartRepository.Create(shoppingCart));
        // Assert
        Assert.Equal(3, _context.ShoppingCarts.ToList().Count);
        var customerShoppingCart = _context
            .Customers
            .Include(c => c.ShoppingCart)
            .First(c => c.CustomerId == 3000)
            .ShoppingCart;
        Assert.NotNull(customerShoppingCart);
    }
    
    [Fact]
    public async void Test_Read_ShoppingCart() {
        // Arrange
        // In Constructor
        // Act
        var shoppingCarts = await _shoppingCartRepository.Read();
        // Assert
        Assert.Equal(2, shoppingCarts.Count);
        Assert.Equal(3, shoppingCarts.First(sc => sc.CartId == 1000).CartItems.Count);
        Assert.Equal(1, shoppingCarts.First(sc => sc.CartId == 2000).CartItems.Count);
    }
    
    [Fact]
    public async void Test_Update_ShoppingCart() {
        // Arrange
        var shoppingCart = _context
            .ShoppingCarts
            .Include(sc => sc.CartItems)
            .First(sc => sc.CartId == 2000);
        // Assert
        Assert.NotNull(shoppingCart);
        Assert.Equal(1, shoppingCart.CartItems.Count);
        // Act
        shoppingCart
            .CartItems
            .Add(new CartItem {
                CartId = 2000, 
                ProductId = 1000, 
                Quantity = 1
            });
        Assert.True(await _shoppingCartRepository.Update(shoppingCart));
        // Assert
        Assert.Equal(0, _context
            .ShoppingCarts
            .Include(sc => sc.CartItems)
            .First(sc => sc.CartId == 2000)
            .CartItems
            .Count);
        Assert.Equal(2000, _context
            .ShoppingCarts
            .First(sc => sc.CartId == 2000)
            .CustomerId);
    }

    [Fact]
    public async void Test_Delete_ShoppingCart() {
        // Arrange
        var shoppingCart = _context
            .ShoppingCarts
            .First(sc => sc.CartId == 2000);

        const int emptyCartId = 4000;
        _context.ShoppingCarts.Add(new ShoppingCart {
            CartId = emptyCartId, 
            CustomerId = 4000, 
            DateCreated = DateTime.Now
        });
        await _context.SaveChangesAsync();
        var emptyShoppingCart = _context
            .ShoppingCarts
            .First(sc => sc.CartId == emptyCartId);
        // Assert
        Assert.NotNull(shoppingCart);
        Assert.NotNull(emptyShoppingCart);
        // Act
        Assert.False(await _shoppingCartRepository.Delete(shoppingCart));
        Assert.True(await _shoppingCartRepository.Delete(emptyShoppingCart));
        // Assert
        Assert.NotNull(_context
            .ShoppingCarts
            .First(sc => sc.CartId == 2000));
        Assert.Null(_context
            .ShoppingCarts
            .FirstOrDefault(sc => sc.CartId == emptyCartId));
    }
    
    [Fact]
    public async void Test_Find_ShoppingCart() {
        // Arrange
        // In Constructor
        // Act
        var foundShoppingCart = await _shoppingCartRepository.Find(1000);
        // Assert
        Assert.NotNull(foundShoppingCart);
        Assert.Equal(1000, _context
            .ShoppingCarts
            .First(sc => sc.CartId == 1000)
            .CustomerId);
        Assert.Equal(3, _context
            .ShoppingCarts
            .Include(sc => sc.CartItems)
            .First(sc => sc.CartId == 1000)
            .CartItems
            .Count);
    }
}