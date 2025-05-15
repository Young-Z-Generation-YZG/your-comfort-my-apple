using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.PromotionRequest;

public sealed record CreatePromotionEventRequest
{
    [Required]
    [JsonPropertyName("event_title")]
    required public string EventTitle { get; set; }

    [Required]
    [JsonPropertyName("event_description")]
    required public string EventDescription { get; set; }

    [Required]
    [JsonPropertyName("event_state")]
    required public string EventState { get; set; }

    [Required]
    [JsonPropertyName("event_valid_from")]
    required public DateTime EventValidFrom { get; set; }

    [Required]
    [JsonPropertyName("event_valid_to")]
    required public DateTime EventValidTo { get; set; }
}
