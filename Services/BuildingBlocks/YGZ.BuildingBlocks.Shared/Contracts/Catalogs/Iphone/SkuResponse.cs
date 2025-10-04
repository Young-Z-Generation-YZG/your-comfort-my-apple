using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record SkuResponse
{
    required public string Id { get; init; }
    required public string SKUCode { get; init; }
    required public string ModelId { get; init; }
    required public string TenantId { get; init; }
    required public string BranchId { get; init; }
    required public string ProductType { get; init; }
    required public string Model { get; init; }
    required public string Color { get; init; }
    required public string Storage { get; init; }
    required public decimal UnitPrice { get; init; }
    required public int AvailableInStock { get; init; }
    required public int TotalSold { get; init; }
    required public string State { get; init; }
    required public string Slug { get; init; }
    required public DateTime CreatedAt { get; init; }
    required public DateTime UpdatedAt { get; init; }
    public DateTime? DeletedAt { get; init; }
    public string? DeletedBy { get; init; }
    public bool IsDeleted { get; init; }
}
