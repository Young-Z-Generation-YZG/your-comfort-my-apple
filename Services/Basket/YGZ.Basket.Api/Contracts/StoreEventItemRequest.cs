using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Basket.Api.Contracts;

public class StoreEventItemRequest
{
    [Required]
    [JsonPropertyName("event_item_id")]
    public required string EventItemId { get; init; }
}
