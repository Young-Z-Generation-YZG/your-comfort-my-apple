using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Test;

public class TestLoggingRequest
{
    [JsonPropertyName("first_name")]
    public string? FirstName { get; init; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; init; }
}
