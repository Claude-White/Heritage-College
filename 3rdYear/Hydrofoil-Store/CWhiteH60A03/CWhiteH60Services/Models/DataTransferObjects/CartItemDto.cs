namespace CWhiteH60Services.Models.DataTransferObjects;

public class CartItemDto {
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }
}