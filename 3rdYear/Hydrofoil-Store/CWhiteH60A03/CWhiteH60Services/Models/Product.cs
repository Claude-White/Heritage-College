using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CWhiteH60Services.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int ProdCatId { get; set; }

    public string Description { get; set; } = null!;

    public string Manufacturer { get; set; } = null!;

    public int Stock { get; set; }

    public decimal BuyPrice { get; set; }

    public decimal SellPrice { get; set; }

    public string? Notes { get; set; }
    
    public string? ImageName { get; set; }
    
    public byte[]? ImageData { get; set; }

    [JsonIgnore]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [JsonIgnore]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [JsonIgnore]
    public virtual ProductCategory ProdCat { get; set; } = null!;
}
