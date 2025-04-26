using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Auth;

public sealed record ResetPasswordRequest
{
    [JsonPropertyName("email")]
    public required string Email { get; init; }
}
