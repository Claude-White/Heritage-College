using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CWhiteH60Store.Models;

[PrimaryKey(nameof(CartItemId))]
public class CartItem {
    public int CartItemId { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int? Quantity { get; set; }
    [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
    [Column(TypeName = "decimal(8,2)")]
    public decimal? Price { get; set; }

    public virtual ShoppingCart Cart { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}