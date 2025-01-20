namespace CWhiteH60Services.Models.DataTransferObjects;

public class OrderDto {
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateFulfilled { get; set; }

    public decimal? Total { get; set; }

    public decimal? Taxes { get; set; }
    
    public CustomerDto? Customer { get; set; }

    public OrderDto() {
    }

    public OrderDto(Order order) {
        OrderId = order.OrderId;
        CustomerId = order.CustomerId;
        DateCreated = order.DateCreated;
        DateFulfilled = order.DateFulfilled;
        Total = order.Total;
        Taxes = order.Taxes;
        Customer = new CustomerDto(order.Customer);
    }
}