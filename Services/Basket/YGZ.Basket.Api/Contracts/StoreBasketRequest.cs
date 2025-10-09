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
    [JsonPropertyName("model_id")]
    public required string ModelId { get; init; }

    [Required]
    [JsonPropertyName("model")]
    public required ModelRequest Model { get; init; }

    [Required]
    [JsonPropertyName("color")]
    public required ColorRequest Color { get; init; }

    [Required]
    [JsonPropertyName("storage")]
    public required StorageRequest Storage { get; init; }

    [Required]
    [JsonPropertyName("quantity")]
    public required int Quantity { get; init; }
}

public sealed record ColorRequest()
{
    [Required]
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [Required]
    [JsonPropertyName("normalized_name")]
    public required string NormalizedName { get; init; }
}

public sealed record ModelRequest()
{
    [Required]
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [Required]
    [JsonPropertyName("normalized_name")]
    public required string NormalizedName { get; init; }
}

public sealed record StorageRequest()
{
    [Required]
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [Required]
    [JsonPropertyName("normalized_name")]
    public required string NormalizedName { get; init; }
}
