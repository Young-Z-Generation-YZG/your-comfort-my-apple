using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Auth;


public sealed record RefreshAccessTokenRequest
{
    [property: JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; set; }
}
