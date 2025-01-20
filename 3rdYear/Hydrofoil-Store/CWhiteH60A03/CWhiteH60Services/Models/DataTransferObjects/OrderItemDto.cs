namespace CWhiteH60Services.Models.DataTransferObjects;

public class OrderItemDto {
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public OrderItemDto() {
    }

    public OrderItemDto(OrderItem orderItem) {
        OrderItemId = orderItem.OrderItemId;
        OrderId = orderItem.OrderId;
        ProductId = orderItem.ProductId;
        Quantity = orderItem.Quantity;
        Price = orderItem.Price;
    }
}