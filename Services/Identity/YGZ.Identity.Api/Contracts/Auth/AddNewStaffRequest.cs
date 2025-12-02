using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Auth;

public sealed record AddNewStaffRequest
{
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
    public string? TenantId { get; init; }

    [JsonPropertyName("branch_id")]
    public string? BranchId { get; init; }
}
