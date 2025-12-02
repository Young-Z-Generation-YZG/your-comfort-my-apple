using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Auth;

public sealed record AssignRolesRequest
{
    [JsonPropertyName("user_id")]
    [JsonRequired]
    public required string UserId { get; init; }

    [JsonPropertyName("roles")]
    [JsonRequired]
    public required List<string> Roles { get; init; }
}
