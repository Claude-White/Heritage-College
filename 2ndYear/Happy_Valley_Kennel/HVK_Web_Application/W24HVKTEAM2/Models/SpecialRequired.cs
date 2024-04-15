using System.ComponentModel.DataAnnotations;

namespace W24HVKTEAM2.Models {
    public class SpecialRequired : ValidationAttribute {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext) {
            if (string.IsNullOrEmpty((string) value)) {
                return new ValidationResult($"The {validationContext.DisplayName} field is required.");
            }
            return ValidationResult.Success;
        }
    }
}
