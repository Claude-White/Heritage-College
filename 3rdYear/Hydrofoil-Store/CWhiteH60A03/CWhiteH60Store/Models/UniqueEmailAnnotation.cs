using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CWhiteH60Store.Models;

public class UniqueEmailAnnotation : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var userManager = validationContext.GetService(typeof(UserManager<IdentityUser>)) as UserManager<IdentityUser>;

        if (userManager == null)
        {
            return new ValidationResult("UserManager service not available.");
        }

        var newEmail = value?.ToString() ?? string.Empty;

        var customerInstance = (Customer)validationContext.ObjectInstance;

        var existingUser = userManager.FindByIdAsync(customerInstance.UserId).Result;

        var allUsers = userManager.Users.ToList();

        if (existingUser != null)
        {
            allUsers.Remove(existingUser);
        }

        bool emailExists = allUsers.Any(u => u.Email.Equals(newEmail, StringComparison.InvariantCultureIgnoreCase));

        if (emailExists)
        {
            return new ValidationResult("The email is already in use.");
        }

        return ValidationResult.Success;
    }
}