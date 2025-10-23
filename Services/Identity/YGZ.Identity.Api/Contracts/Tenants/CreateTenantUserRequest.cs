using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Tenants;

public sealed record CreateTenantUserRequest
{
    [JsonPropertyName("first_name")]
    public required string FirstName { get; init; }

    [JsonPropertyName("last_name")]
    public required string LastName { get; init; }

    [JsonPropertyName("email")]
    public required string Email { get; init; }

    [JsonPropertyName("password")]
    public required string Password { get; init; }

    [JsonPropertyName("phone_number")]
    public required string PhoneNumber { get; init; }
}