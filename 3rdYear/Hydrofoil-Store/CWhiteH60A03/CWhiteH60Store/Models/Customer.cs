using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CWhiteH60Store.Models;

[PrimaryKey(nameof(CustomerId))]
public class Customer {
    public int CustomerId { get; set; }
    
    [RegexStringValidator("^[a-zA-Z'-\\s]+$")]
    [StringLength(20)]
    public string? FirstName { get; set; }
    
    [RegexStringValidator("^[a-zA-Z'-\\s]+$")]
    [StringLength(30)]
    public string? LastName { get; set; }
    
    [DataType(DataType.EmailAddress)]
    [StringLength(30)]
    [UniqueEmailAnnotation]
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
    
    public string UserId { get; set; }
    
    public string? Password { get; set; }
    
    [DisplayName("Confirm Password")]
    [Compare("Password")]
    [NotMapped]
    public string ConfirmPassword { get; set; }

    public virtual ShoppingCart? Cart { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}