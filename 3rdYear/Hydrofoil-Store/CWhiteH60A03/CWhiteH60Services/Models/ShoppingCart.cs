using System;
using System.Collections.Generic;
using CWhiteH60Services.Models.DataTransferObjects;
using Newtonsoft.Json;

namespace CWhiteH60Services.Models;

public partial class ShoppingCart
{
    public int CartId { get; set; }

    public int CustomerId { get; set; }

    public DateTime? DateCreated { get; set; }

    [JsonIgnore]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [JsonIgnore]
    public virtual Customer Customer { get; set; } = null!;

    public ShoppingCart() {
    }

    public ShoppingCart(ShoppingCartDto shoppingCartDto) {
        CartId = shoppingCartDto.CartId;
        CustomerId = shoppingCartDto.CustomerId;
        DateCreated = shoppingCartDto.DateCreated;
        CartItems = shoppingCartDto.CartItems.Select(ci => new CartItem(ci)).ToList();
    }
}
