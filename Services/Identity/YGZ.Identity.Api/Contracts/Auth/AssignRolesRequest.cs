using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Auth;

public sealed record AssignRolesRequest
{
    [JsonPropertyName("roles")]
    [JsonRequired]
    public required List<string> Roles { get; init; }
}
