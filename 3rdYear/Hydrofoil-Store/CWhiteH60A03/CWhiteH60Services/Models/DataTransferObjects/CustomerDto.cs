using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace CWhiteH60Services.Models.DataTransferObjects;

public class CustomerDto {
    public int CustomerId { get; set; }
    
    [RegexStringValidator("^[a-zA-Z'-\\s]+$")]
    [StringLength(20)]
    public string? FirstName { get; set; }
    
    [RegexStringValidator("^[a-zA-Z'-\\s]+$")]
    [StringLength(30)]
    public string? LastName { get; set; }
    
    [DataType(DataType.EmailAddress)]
    [StringLength(30)]
    public string Email { get; set; }
    
    [DataType(DataType.PhoneNumber)]
    [MaxLength(10)]
    [MinLength(10)]
    [StringLength(10)]
    public string? PhoneNumber { get; set; }
    
    [MaxLength(2)]
    [StringLength(2)]
    public string? Province { get; set; }
    
    [DataType(DataType.CreditCard)]
    [StringLength(16)]
    public string? CreditCard { get; set; }
    
    public string? UserId { get; set; }
    
    public string? Password { get; set; }

    public CustomerDto() {
    }

    public CustomerDto(Customer customer) {
        CustomerId = customer.CustomerId;
        FirstName = customer.FirstName;
        LastName = customer.LastName;
        Email = customer.Email;
        PhoneNumber = customer.PhoneNumber;
        Province = customer.Province;
        CreditCard = customer.CreditCard;
    }
}