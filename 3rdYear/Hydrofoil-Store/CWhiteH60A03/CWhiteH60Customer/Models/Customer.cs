using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CWhiteH60Customer.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }

    [UniqueEmailAnnotation]
    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    [Required]
    public string? Province { get; set; }

    [Required]
    [ValidateCard]
    public string? CreditCard { get; set; }

    public string UserId { get; set; } = null!;

    public string? Password { get; set; }
    
    [NotMapped]
    [Required]
    public string? Address { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ShoppingCart? ShoppingCart { get; set; }
}
