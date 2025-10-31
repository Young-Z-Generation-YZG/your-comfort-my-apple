using System.Text.Json.Serialization;

namespace YGZ.BuildingBlocks.Shared.Contracts.Keycloak;

public sealed record TokenExchangeResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; init; }

    [JsonPropertyName("expires_in")]
    public int? ExpiresIn { get; init; }

    [JsonPropertyName("refresh_expires_in")]
    public int? RefreshExpiresIn { get; init; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; init; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; init; }

    [JsonPropertyName("not-before-policy")]
    public int? NotBeforePolicy { get; init; }

    [JsonPropertyName("session_state")]
    public string? SessionState { get; init; }

    [JsonPropertyName("scope")]
    public string? Scope { get; init; }

    [JsonPropertyName("issued_token_type")]
    public string? IssuedTokenType { get; init; }
}