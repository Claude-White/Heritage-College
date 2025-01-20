using CWhiteH60Store.Models;
using Microsoft.AspNetCore.Identity;

namespace CWhiteH60Store;

public class CreateUserRolesAndAdminUsers {
    public static async Task Execute(WebApplication app) {
        using (var scope = app.Services.CreateScope()) {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<H60AssignmentDbCWContext>();

            string[] roleNames = { "customer", "clerk", "manager" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames) {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist) {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var managerUser = await userManager.FindByEmailAsync("manager@gmail.com");
            if (managerUser == null) {
                managerUser = new IdentityUser() {
                    UserName = "manager@gmail.com",
                    Email = "manager@gmail.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(managerUser, "?Manager1!");
                await userManager.AddToRoleAsync(managerUser, "manager");
            }
            
            var clerkUser = await userManager.FindByEmailAsync("clerk@gmail.com");
            if (clerkUser == null) {
                clerkUser = new IdentityUser() {
                    UserName = "clerk@gmail.com",
                    Email = "clerk@gmail.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(clerkUser, "?Clerk1!");
                await userManager.AddToRoleAsync(clerkUser, "clerk");
            }
            
            var customerUser = await userManager.FindByEmailAsync("customer@gmail.com");
            if (customerUser == null) {
                customerUser = new IdentityUser() {
                    UserName = "customer@gmail.com",
                    Email = "customer@gmail.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(customerUser, "?Customer1!");
                await userManager.AddToRoleAsync(customerUser, "customer");
                
                var userId = await userManager.GetUserIdAsync(customerUser);
                var customer = new Customer() {
                    Email = customerUser.Email,
                    UserId = userId
                };
                dbContext.Customers.Add(customer);
                dbContext.SaveChanges();
                
                var newCustomer = dbContext.Customers.FirstOrDefault(x => x.Email == customerUser.Email);
                if (newCustomer != null) {
                    var shoppingCart = new ShoppingCart {
                        CustomerId = newCustomer.CustomerId,
                        DateCreated = DateTime.Now,
                    };
                    dbContext.ShoppingCarts.Add(shoppingCart);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}