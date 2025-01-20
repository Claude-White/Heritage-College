using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Newtonsoft.Json;

namespace CWhiteH60Services.Models;

public partial class Customer
{
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

    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [JsonIgnore]
    public virtual ShoppingCart? ShoppingCart { get; set; }
}
