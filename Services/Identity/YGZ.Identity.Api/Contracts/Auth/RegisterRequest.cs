using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Auth;

public sealed record RegisterRequest
{ 
    [JsonPropertyName("email")]
    public required string Email { get; init; }

    [JsonPropertyName("password")]
    public required string Password { get; init; }

    [JsonPropertyName("confirm_password")]
    public required string ConfirmPassword { get; init; }

    [JsonPropertyName("first_name")]
    public required string FirstName { get; init; }

    [JsonPropertyName("last_name")]
    public required string LastName { get; init; }

    [JsonPropertyName("phone_number")]
    public required string PhoneNumber { get; init; }

    [JsonPropertyName("birthday")]
    public required string BirthDay { get; init; }

    [JsonPropertyName("country")]
    public required string Country { get; init; }
}