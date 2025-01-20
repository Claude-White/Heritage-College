using CWhiteH60Services.Models;
using Microsoft.EntityFrameworkCore;

namespace CWhiteH60Services.DAL;

public class ShoppingCartRepository : IShoppingCartRepository<ShoppingCart> {
    private readonly H60AssignmentDbCWContext _context;

    public ShoppingCartRepository(H60AssignmentDbCWContext context) {
        _context = context;
    }
    
    public async Task<bool> Create(ShoppingCart shoppingCart) {
        shoppingCart.CartItems = new List<CartItem>();
        await _context.ShoppingCarts.AddAsync(shoppingCart);
        var rowsAffected = await _context.SaveChangesAsync();
        return rowsAffected > 0;
    }

    public async Task<List<ShoppingCart>> Read() {
        return await _context
            .ShoppingCarts
            .Include(sc => sc.CartItems)
            .ToListAsync();
    }

    public async Task<bool> Update(ShoppingCart shoppingCart) {
        shoppingCart.CartItems = new List<CartItem>();
        _context.ShoppingCarts.Update(shoppingCart);
        var rowsAffected = await _context.SaveChangesAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> Delete(ShoppingCart shoppingCart) {
        if (shoppingCart.CartItems.Count != 0) {
            return false;
        }
        _context.ShoppingCarts.Remove(shoppingCart);
        var rowsAffected = await _context.SaveChangesAsync();
        return rowsAffected > 0;
    }

    public async Task<ShoppingCart> Find(int id) {
        return await _context
            .ShoppingCarts
            .Include(sc => sc.Customer)
            .Include(sc => sc.CartItems)
            .FirstOrDefaultAsync(sc => sc.CartId == id);
    }

    public async Task<ShoppingCart> FindByCustomerId(int customerId) {
        return await _context
            .ShoppingCarts
            .Include(sc => sc.Customer)
            .Include(sc => sc.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(sc => sc.CustomerId == customerId);
    }
}