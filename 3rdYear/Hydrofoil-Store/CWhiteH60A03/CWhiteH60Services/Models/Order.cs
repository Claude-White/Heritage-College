using System;
using System.Collections.Generic;
using CWhiteH60Services.Models.DataTransferObjects;
using Newtonsoft.Json;

namespace CWhiteH60Services.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateFulfilled { get; set; }

    public decimal? Total { get; set; }

    public decimal? Taxes { get; set; }

    [JsonIgnore]
    public virtual Customer Customer { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public Order() {
    }

    public Order(OrderDto orderDto) {
        OrderId = orderDto.OrderId;
        CustomerId = orderDto.CustomerId;
        DateCreated = orderDto.DateCreated;
        DateFulfilled = orderDto.DateFulfilled;
        Total = orderDto.Total;
        Taxes = orderDto.Taxes;
    }
}
