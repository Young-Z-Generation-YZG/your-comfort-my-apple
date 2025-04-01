

using System.Text.Json.Serialization;

namespace YGZ.BuildingBlocks.Shared.Contracts.Auth;

public sealed record RefreshAccessTokenResponse
{
    [property: JsonPropertyName("access_token")]
    required public string AccessToken { get; init; }

    [property: JsonPropertyName("access_token_expires_in_seconds")]
    required public int AccessTokenExpiresInSeconds { get; init; }
}
