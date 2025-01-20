using Microsoft.EntityFrameworkCore;

namespace CWhiteH60Store.Models;

[PrimaryKey(nameof(CartId))]
public class ShoppingCart {
    public int CartId { get; set; }
    public int CustomerId { get; set; }
    public DateTime? DateCreated { get; set; }

    public virtual Customer Customer { get; set; } = null!;
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}