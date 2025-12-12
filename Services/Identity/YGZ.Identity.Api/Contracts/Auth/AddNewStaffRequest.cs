using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Auth;

public sealed record AddNewStaffRequest
{
    [JsonPropertyName("birth_day")]
    [JsonRequired]
    public required string BirthDay { get; init; }

    [JsonPropertyName("email")]
    [JsonRequired]
    public required string Email { get; init; }

    [JsonPropertyName("password")]
    [JsonRequired]
    public required string Password { get; init; }

    [JsonPropertyName("first_name")]
    [JsonRequired]
    public required string FirstName { get; init; }

    [JsonPropertyName("last_name")]
    [JsonRequired]
    public required string LastName { get; init; }

    [JsonPropertyName("phone_number")]
    [JsonRequired]
    public required string PhoneNumber { get; init; }

    [JsonPropertyName("role_name")]
    [JsonRequired]
    public required string RoleName { get; init; }

    [JsonPropertyName("tenant_id")]
    [JsonRequired]
    public required string TenantId { get; init; }

    [JsonPropertyName("branch_id")]
    [JsonRequired]
    public required string BranchId { get; init; }
}
