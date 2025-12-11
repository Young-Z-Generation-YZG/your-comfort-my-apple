using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Basket.Api.Contracts;

public sealed record StoreBasketRequest
{
    [Required]
    [JsonPropertyName("cart_items")]
    public required List<CartItemRequest> CartItems { get; init; }
}

public sealed record CartItemRequest()
{
    [Required]
    [JsonPropertyName("is_selected")]
    public required bool IsSelected { get; init; }

    [Required]
    [JsonPropertyName("sku_id")]
    public required string SkuId { get; init; }

    [Required]
    [JsonPropertyName("quantity")]
    public required int Quantity { get; init; }
}


