using CWhiteH60Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CWhiteH60Services.DAL;

public class CustomerRepository : ICustomerRepository<Customer> {
    private readonly H60AssignmentDbCWContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;
    private readonly IUserEmailStore<IdentityUser> _emailStore;

    public CustomerRepository(H60AssignmentDbCWContext context, UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore, IUserEmailStore<IdentityUser> emailStore) {
        _context = context;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = emailStore;
    }
    
    public async Task Create(Customer customer) {
        var user = Activator.CreateInstance<IdentityUser>();
        user.EmailConfirmed = true;
        await _userStore.SetUserNameAsync(user, customer.Email, CancellationToken.None);
        await _userManager.SetPhoneNumberAsync(user, customer.PhoneNumber);
        await _emailStore.SetEmailAsync(user, customer.Email, CancellationToken.None);
        var result = await _userManager.CreateAsync(user, customer.Password!);
        if (result.Succeeded) {
            await _userManager.AddToRoleAsync(user, "customer");
        }
        customer.UserId = user.Id;
        
        // Don't save customer password as string
        customer.Password = null;

        _context.Customers.Add(customer);
    }

    public async Task Update(Customer customer) {
        var user = await _userManager.FindByIdAsync(customer.UserId);

        if (user == null) {
            return;
        }
        
        user.Email = customer.Email;
        user.UserName = customer.Email;
        
        if (customer.PhoneNumber != null) {
            user.PhoneNumber = customer.PhoneNumber;
        }
        
        await _userManager.UpdateAsync(user);
        _context.Customers.Update(customer);
    }

    public async Task Delete(Customer customer) {
        var existingCustomer = await GetById(customer.CustomerId);
        var user = await _userManager.FindByIdAsync(existingCustomer.UserId);
        if (user == null) {
            return;
        }
        
        // _context.Customers.Remove(customer);
        await _userManager.DeleteAsync(user);
    }

    public List<Customer> GetAll() {
        return _context.Customers
            .Include(c => c.Orders)
            .Include(c => c.ShoppingCart)
            .OrderBy(c => c.FirstName)
            .ThenBy(c => c.LastName)
            .ToList();
    }

    public async Task<Customer> GetByUserId(string userId) {
        return _context.Customers.FirstOrDefault(c => c.UserId.Equals(userId));
    }

    public async Task<Customer> GetById(int id) {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == id);
    }

    public async Task<Customer> GetByIdInclude(int id) {
        return await _context.Customers
            .Include(c => c.Orders)
            .Include(c => c.ShoppingCart)
            .FirstOrDefaultAsync(c => c.CustomerId == id);
    }

    public async Task Save() {
        await _context.SaveChangesAsync();
    }

    public bool NameExists(string name, int id) {
        // Many customers can have same name
        return _context.Customers.Any(c => c.CustomerId == id);
    }
}