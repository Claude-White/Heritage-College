using System;
using System.Collections.Generic;
using CWhiteH60Services.Models.DataTransferObjects;
using Newtonsoft.Json;

namespace CWhiteH60Services.Models;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    [JsonIgnore]
    public virtual ShoppingCart Cart { get; set; } = null!;

    [JsonIgnore]
    public virtual Product Product { get; set; } = null!;

    public CartItem() {
    }

    public CartItem(CartItemDto CartItemDto) {
        CartItemId = CartItemDto.CartItemId;
        CartId = CartItemDto.CartId;
        ProductId = CartItemDto.ProductId;
        Quantity = CartItemDto.Quantity;
        Price = CartItemDto.Price;
    }
}
