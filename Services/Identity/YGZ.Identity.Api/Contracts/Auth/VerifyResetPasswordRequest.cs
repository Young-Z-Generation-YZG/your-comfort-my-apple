

using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Auth;

public sealed record VerifyResetPasswordRequest
{
    [JsonPropertyName("email")]
    public required string Email { get; init; }

    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("new_password")]
    public required string NewPassword { get; init; }

    [JsonPropertyName("confirm_password")]
    public required string ConfirmPassword { get; init; }
}
