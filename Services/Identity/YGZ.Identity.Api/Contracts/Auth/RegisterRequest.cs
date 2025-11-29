using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Auth;

public sealed record RegisterRequest
{
    [JsonPropertyName("email")]
    [JsonRequired]
    public required string Email { get; init; }

    [JsonPropertyName("password")]
    [JsonRequired]
    public required string Password { get; init; }

    [JsonPropertyName("confirm_password")]
    [JsonRequired]
    public required string ConfirmPassword { get; init; }

    [JsonPropertyName("first_name")]
    [JsonRequired]
    public required string FirstName { get; init; }

    [JsonPropertyName("last_name")]
    [JsonRequired]
    public required string LastName { get; init; }

    [JsonPropertyName("phone_number")]
    [JsonRequired]
    public required string PhoneNumber { get; init; }

    [JsonPropertyName("birth_date")]
    [JsonRequired]
    public required string BirthDate { get; init; }

    [JsonPropertyName("country")]
    [JsonRequired]
    public required string Country { get; init; }
}