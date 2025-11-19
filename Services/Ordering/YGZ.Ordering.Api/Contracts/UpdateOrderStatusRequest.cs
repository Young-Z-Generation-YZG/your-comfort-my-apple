using System.Text.Json.Serialization;

namespace YGZ.Ordering.Api.Contracts;

public sealed record UpdateOrderStatusRequest
{
    [JsonPropertyName("update_status")]
    public required string UpdateStatus { get; init; }
}
