using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Basket.Api.Contracts;

public class StoreEventItemRequest
{
    [Required]
    [JsonPropertyName("event_item_id")]
    public required string EventItemId { get; init; }

    [Required]
    [JsonPropertyName("product_information")]
    public required ProductInformationRequest ProductInformation { get; init; }
}

public class ProductInformationRequest
{
    [Required]
    [JsonPropertyName("product_name")]
    public required string ProductName { get; init; }

    [Required]
    [JsonPropertyName("model_normalized_name")]
    public required string ModelNormalizedName { get; init; }

    [Required]
    [JsonPropertyName("color_normalized_name")]
    public required string ColorNormalizedName { get; init; }

    [Required]
    [JsonPropertyName("storage_normalized_name")]
    public required string StorageNormalizedName { get; init; }

    [Required]
    [JsonPropertyName("display_image_url")]
    public required string DisplayImageUrl { get; init; }
}
