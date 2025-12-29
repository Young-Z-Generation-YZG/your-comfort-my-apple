using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Auth;

public sealed record LoginRequest
{
    [JsonPropertyName("email")]
    public required string Email { get; init; }

    [JsonPropertyName("password")]
    public required string Password { get; init; }
}