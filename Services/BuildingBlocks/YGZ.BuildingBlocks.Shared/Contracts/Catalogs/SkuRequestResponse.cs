using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record SkuRequestResponse
{
    public required string Id { get; init; }
    public required string SenderUserId { get; init; }
    public required EmbeddedBranchResponse FromBranch { get; init; }
    public required EmbeddedBranchResponse ToBranch { get; init; }
    public required EmbeddedSkuResponse Sku { get; init; }
    public required int RequestQuantity { get; init; }
    public required string State { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }
    public string? DeletedBy { get; init; }
}
