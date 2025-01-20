namespace CWhiteH60Services.Models.DataTransferObjects;

public class ShoppingCartDto {
    public int CartId { get; set; }

    public int CustomerId { get; set; }

    public DateTime? DateCreated { get; set; }
    
    public virtual ICollection<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
}