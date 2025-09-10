using System.Text.Json.Serialization;

namespace YGZ.BuildingBlocks.Shared.Contracts.Auth;

public sealed record TokenResponse
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; init; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init; }

    [JsonPropertyName("refresh_expires_in")]
    public int RefreshExpiresIn { get; init; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; init; }

    [JsonPropertyName("not-before-policy")]
    public int NotBeforePolicy { get; init; }

    [JsonPropertyName("token_type")]
    public required string TokenType { get; init; }

    [JsonPropertyName("session_state")]
    public string? SessionState { get; init; }

    [JsonPropertyName("scope")]
    public required string Scope { get; init; }
}
