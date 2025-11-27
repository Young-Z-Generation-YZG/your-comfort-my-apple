using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Ordering.Api.Contracts;

public sealed record CreateNotificationRequest
{
    [Required]
    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [Required]
    [JsonPropertyName("content")]
    public required string Content { get; init; }

    [Required]
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [Required]
    [JsonPropertyName("status")]
    public required string Status { get; init; }

    [Required]
    [JsonPropertyName("link")]
    public required string Link { get; init; }
}
