using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Auth.Keycloak;

public sealed record AuthorizationCodeRequest
{
    [JsonPropertyName("code")]
    public required string Code { get; init; }
}
