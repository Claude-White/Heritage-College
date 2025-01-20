using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CWhiteH60Store.Models;

public partial class Product
{
    public int ProductID { get; set; }

    public int ProdCatId { get; set; }

    [Required]
    [MaxLength(80)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(80)]
    public string? Manufacturer { get; set; }
    
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Required]
    [HigherSellPrice]
    [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
    public decimal? BuyPrice { get; set; }

    [Required]
    [HigherSellPrice]
    [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
    public decimal? SellPrice { get; set; }
    
    public string? Notes { get; set; }
    
    public string? ImageName { get; set; }
    
    public byte[]? ImageData { get; set; }

    public virtual ProductCategory ProdCat { get; set; } = null!;
}