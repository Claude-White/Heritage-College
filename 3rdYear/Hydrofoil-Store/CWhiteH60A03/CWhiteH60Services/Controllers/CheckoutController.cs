using CWhiteH60Services.CalculateTaxes;
using CWhiteH60Services.CheckCreditCard;
using Microsoft.AspNetCore.Mvc;

namespace CWhiteH60Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckoutController : ControllerBase {
    private readonly CheckCreditCardSoapClient _creditCardClient;
    private readonly CalculateTaxesSoapClient _calculateTaxesClient;

    public CheckoutController(CheckCreditCardSoapClient creditCardClient, CalculateTaxesSoapClient calculateTaxesClient) {
        _creditCardClient = creditCardClient;
        _calculateTaxesClient = calculateTaxesClient;
    }

    [HttpPost("CheckCreditCard")]
    public async Task<ActionResult> CheckCreditCard(CreditCard creditCard) {
        var response = await _creditCardClient.CreditCardCheckAsync(creditCard.CardNumber);
        switch (response) {
            case -1: return BadRequest("Invalid card number length (12 to 16 numbers)");
            case -2: return BadRequest("Not all numbers");
            case -3: return BadRequest("Invalid card number (sum of each 4 < 30");
            case -4: return BadRequest("Product of last 2 digits must be multiple of 2");
            case -5: return BadRequest("Card balance insufficient");
            default: return Ok();
        }
    }

    public class CreditCard {
        public string CardNumber { get; set; }
    }

    [HttpGet("CalculateTax/{amount}/{province}")]
    public async Task<ActionResult> CalculateTaxes(double amount, string province) {
        if (amount < 0) {
            return BadRequest("Invalid amount (must be greater than zero)");
        }
        
        if (province.Equals("mb", StringComparison.InvariantCultureIgnoreCase)) {
            return Ok(amount * 0.12);
        }
        var response = await _calculateTaxesClient.CalculateTaxAsync(amount, province);
        if (response == -1) {
            return BadRequest("Invalid province");
        }
        return Ok(response);
    }
}