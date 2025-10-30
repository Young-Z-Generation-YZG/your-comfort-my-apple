using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record SkuWithImageResponse
{
    public required string Id { get; init; }
    public required string Code { get; init; }
    public required string ModelId { get; init; }
    public required string TenantId { get; init; }
    public required string BranchId { get; init; }
    public required string ProductClassification { get; init; }
    public required ModelResponse Model { get; init; }
    public required ColorResponse Color { get; init; }
    public required StorageResponse Storage { get; init; }
    public required string DisplayImageUrl { get; init; }
    public required decimal UnitPrice { get; init; }
    public required int AvailableInStock { get; init; }
    public required int TotalSold { get; init; }
    public required ReservedForEventResponse? ReservedForEvent { get; init; }
    public required string State { get; init; }
    public required string Slug { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public DateTime? DeletedAt { get; init; }
    public string? DeletedBy { get; init; }
    public bool IsDeleted { get; init; }
}
