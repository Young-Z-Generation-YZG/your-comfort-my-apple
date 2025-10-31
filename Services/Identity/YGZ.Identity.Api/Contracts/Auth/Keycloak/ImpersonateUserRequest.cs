using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Auth.Keycloak;

public sealed record ImpersonateUserRequest
{
    [JsonPropertyName("user_id")]
    [JsonRequired]
    public required string UserId { get; init; }
}
