using System;
using System.Collections.Generic;
using CWhiteH60Services.Models.DataTransferObjects;
using Newtonsoft.Json;

namespace CWhiteH60Services.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    [JsonIgnore]
    public virtual Order Order { get; set; } = null!;

    [JsonIgnore]
    public virtual Product Product { get; set; } = null!;
    
    public OrderItem() {
    }

    public OrderItem(OrderItemDto orderItemDto) {
        OrderItemId = orderItemDto.OrderItemId;
        OrderId = orderItemDto.OrderId;
        ProductId = orderItemDto.ProductId;
        Quantity = orderItemDto.Quantity;
        Price = orderItemDto.Price;
    }

    public OrderItem(CartItem cartItem) {
        ProductId = cartItem.ProductId;
        Quantity = cartItem.Quantity;
        Price = cartItem.Price;
    }
}
