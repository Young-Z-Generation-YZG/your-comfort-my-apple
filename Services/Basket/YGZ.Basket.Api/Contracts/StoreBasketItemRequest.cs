using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Basket.Api.Contracts;

public sealed record StoreBasketItemRequest
{
    [Required]
    [JsonPropertyName("sku_id")]
    public required string SkuId { get; init; }

    [Required]
    [JsonPropertyName("quantity")]
    public required int Quantity { get; init; }
}
