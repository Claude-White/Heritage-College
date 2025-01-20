using CWhiteH60Services.Models;
using Microsoft.EntityFrameworkCore;

namespace CWhiteH60Services.DAL;

public class OrderItemRepository : IOrderItemRepository<OrderItem> {
    private readonly H60AssignmentDbCWContext _context;

    public OrderItemRepository(H60AssignmentDbCWContext context) {
        _context = context;
    }
    
    public async Task<bool> Create(OrderItem orderItem) {
        orderItem.Order = null;
        orderItem.Product = null;
        await _context.OrderItems.AddAsync(orderItem);
        var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderItem.OrderId);
        var cartItem = _context.CartItems.Include(ci => ci.Cart).FirstOrDefault(x => x.ProductId == orderItem.ProductId && order.CustomerId == x.Cart.CustomerId);
        if (cartItem != null) {
            _context.CartItems.Remove(cartItem);
        }
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<OrderItem>> Read() {
        return await _context.OrderItems
            .Include(oi => oi.Product)
            .Include(oi => oi.Order)
            .ToListAsync();
    }

    public async Task<bool> Update(OrderItem orderItem) {
        orderItem.Order = null;
        orderItem.Product = null;
        _context.OrderItems.Update(orderItem);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<OrderItem> Find(int id) {
        return await _context.OrderItems
            .Include(oi => oi.Product)
            .Include(oi => oi.Order)
            .FirstOrDefaultAsync(oi => oi.OrderItemId == id);
    }

    public async Task<List<OrderItem>> FindByOrderId(int id) {
        return await _context.OrderItems
            .Include(oi => oi.Product)
            .Include(oi => oi.Order)
            .Where(oi => oi.OrderId == id)
            .ToListAsync();
    }
}