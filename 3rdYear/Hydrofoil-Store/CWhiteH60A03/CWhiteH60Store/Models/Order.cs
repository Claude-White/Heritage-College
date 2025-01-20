using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CWhiteH60Store.Models;

[PrimaryKey(nameof(OrderId))]
public class Order {
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateFulfilled { get; set; }
    [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
    [Column(TypeName = "decimal(10,2)")]
    public decimal? Total { get; set; }
    [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
    [Column(TypeName = "decimal(8,2)")]
    public decimal? Taxes { get; set; }

    public virtual Customer Customer { get; set; } = null!;
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}