using System;
using System.Collections.Generic;

namespace CWhiteH60Customer.Models;

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

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ProductCategory ProdCat { get; set; } = null!;
}
