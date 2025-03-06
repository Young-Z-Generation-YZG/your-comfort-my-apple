

using System.Text.Json.Serialization;

namespace YGZ.BuildingBlocks.Shared.Contracts.Auth;

public sealed record TokenResponse([property: JsonPropertyName("access_token")] string AccessToken,
                                   [property: JsonPropertyName("expires_in")] int ExpiresIn,
                                   [property: JsonPropertyName("refresh_expires_in")] int RefreshExpiresIn,
                                   [property: JsonPropertyName("token_type")] string TokenType,
                                   [property: JsonPropertyName("scope")] string Scope) { }