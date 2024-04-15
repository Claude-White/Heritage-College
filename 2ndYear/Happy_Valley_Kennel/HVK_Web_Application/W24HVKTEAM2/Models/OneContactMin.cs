using System.ComponentModel.DataAnnotations;

namespace W24HVKTEAM2.Models {
    public class OneContactMin : ValidationAttribute {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
            var email = (string)validationContext.ObjectType.GetProperty("Email").GetValue(validationContext.ObjectInstance);
            var phone = (string)validationContext.ObjectType.GetProperty("Phone").GetValue(validationContext.ObjectInstance);
            var cellphone = (string)validationContext.ObjectType.GetProperty("CellPhone").GetValue(validationContext.ObjectInstance);
            if (string.IsNullOrEmpty(cellphone) && string.IsNullOrEmpty(phone) && string.IsNullOrEmpty(email)) {
                return new ValidationResult("Enter in at least one of the following: Email, Phone or Cell Phone");
            }
            return ValidationResult.Success;
        }
    }
}
