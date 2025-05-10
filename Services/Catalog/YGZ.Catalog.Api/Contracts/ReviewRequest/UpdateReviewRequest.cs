using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.ReviewRequest;

public sealed record UpdateReviewRequest
{
    [JsonPropertyName("content")]
    public required string Content { get; set; }

    [JsonPropertyName("rating")]
    public required int Rating { get; set; }
}
