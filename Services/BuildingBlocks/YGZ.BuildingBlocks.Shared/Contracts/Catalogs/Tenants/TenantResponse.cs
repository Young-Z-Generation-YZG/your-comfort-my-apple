using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Tenants;


[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record TenantResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string SubDomain { get; init; }
    public string? Description { get; init; }
    public required string TenantType { get; init; }
    public required string TenantState { get; init; }
    public BranchResponse? EmbeddedBranch { get; init; }
    public DateTime? CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }
    public string? DeletedBy { get; init; }

}
