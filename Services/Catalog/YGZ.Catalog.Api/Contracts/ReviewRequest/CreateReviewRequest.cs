using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.ReviewRequest;


public class CreateReviewRequest
{
    [Required]
    [JsonPropertyName("sku_id")]
    public required string SkuId { get; init; }

    [Required]
    [JsonPropertyName("order_id")]
    public required string OrderId { get; init; }

    [Required]
    [JsonPropertyName("order_item_id")]
    public required string OrderItemId { get; init; }
    
    [Required]
    [JsonPropertyName("content")]
    public required string Content { get; init; }

    [Required]
    [JsonPropertyName("rating")]
    public required int Rating { get; init; }

}
