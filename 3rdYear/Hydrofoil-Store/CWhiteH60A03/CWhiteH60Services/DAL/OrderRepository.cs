using CWhiteH60Services.Models;
using Microsoft.EntityFrameworkCore;

namespace CWhiteH60Services.DAL;

public class OrderRepository : IOrderRepository<Order> {
    private readonly H60AssignmentDbCWContext _context;

    public OrderRepository(H60AssignmentDbCWContext context) {
        _context = context;
    }
    
    public async Task<bool> Create(Order order) {
        order.OrderItems = null;
        order.Customer = null;
        await _context.Orders.AddAsync(order);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<Order>> Read() {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.Customer)
            .ToListAsync();
    }

    public async Task<List<Order>> ReadByDate(DateTime date) {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.Customer)
            .Where(o => o.DateFulfilled.Value.Date == date.Date)
            .ToListAsync();
    }

    public async Task<List<Order>> ReadByCustomer(string customerName) {
        var normalizedCustomerName = customerName.ToLowerInvariant();
    
        return await _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.Customer)
            .Where(o => (o.Customer.FirstName.ToLower() + " " + o.Customer.LastName.ToLower()) == normalizedCustomerName)
            .ToListAsync();
    }

    public async Task<List<Order>> ReadByCustomer(int customerId) {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.Customer)
            .Where(o => o.Customer.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<bool> Update(Order order) {
        order.OrderItems = null;
        order.Customer = null;
        _context.Orders.Update(order);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Order> Find(int id) {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.OrderId == id);
    }
}