using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Auth;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record GetIdentityResponse
{
    public required string Id { get; init; }
    public string? TenantId { get; init; }
    public string? BranchId { get; init; }
    public string? TenantSubDomain { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required bool EmailConfirmed { get; init; }
    public required string PhoneNumber { get; init; }
    public required List<string> Roles { get; init; }
}
