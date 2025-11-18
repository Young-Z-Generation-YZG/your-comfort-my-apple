using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.PromotionRequest;

public sealed record CreateEventRequest
{
    [Required]
    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [Required]
    [JsonPropertyName("description")]
    public required string Description { get; init; }

    [Required]
    [JsonPropertyName("start_date")]
    public required DateTime StartDate { get; init; }

    [Required]
    [JsonPropertyName("end_date")]
    public required DateTime EndDate { get; init; }
}
