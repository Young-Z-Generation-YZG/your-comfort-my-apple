using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.ReviewRequest;


public class CreateReviewRequest
{
    [Required]
    [JsonPropertyName("product_id")]
    required public string ProductId { get; set; }

    [Required]
    [JsonPropertyName("model_id")]
    required public string ModelId { get; set; }

    [Required]
    [JsonPropertyName("content")]
    required public string Content { get; set; }

    [Required]
    [JsonPropertyName("rating")]
    required public int Rating { get; set; }

    [Required]
    [JsonPropertyName("order_item_id")]
    required public string OrderItemId { get; set; }
}
