
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Identity;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record UserResponse
{
    public required string Id { get; init; }
    public  string? TenantId { get; init; }
    public  string? BranchId { get; init; }
    public required string UserName { get; init; }
    public required string NormalizedUserName { get; init; }
    public required string Email { get; init; }
    public required string NormalizedEmail { get; init; }
    public required bool EmailConfirmed { get; init; }
    public required string PhoneNumber { get; init; }
    public ProfileResponse? Profile { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }
    public required bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }
    public string? DeletedBy { get; init; }
}
