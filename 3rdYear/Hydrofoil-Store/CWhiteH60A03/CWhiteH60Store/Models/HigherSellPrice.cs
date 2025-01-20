using System.ComponentModel.DataAnnotations;

namespace CWhiteH60Store.Models;

public class HigherSellPrice : ValidationAttribute {

    protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
        var tempBuyPrice = validationContext.ObjectType.GetProperty("BuyPrice").GetValue(validationContext.ObjectInstance);
        var tempSellPrice = validationContext.ObjectType.GetProperty("SellPrice").GetValue(validationContext.ObjectInstance);
        
        if (tempBuyPrice == null || tempSellPrice == null)
        {
            return new ValidationResult("Buy Price or Sell Price was not found.");
        }

        var buyPrice = (decimal)tempBuyPrice;
        var sellPrice = (decimal)tempSellPrice;
        return buyPrice > sellPrice ? new ValidationResult($"Sell Price must be greater than Buy Price") : ValidationResult.Success;
    }
}