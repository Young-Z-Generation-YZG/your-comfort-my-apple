using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Discounts;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record EventItemResponse
{
    public required string Id { get; init; }
    public required string EventId { get; init; }
    public required string SkuId { get; init; }
    public required string TenantId { get; init; }
    public required string BranchId { get; init; }
    public string? ModelName { get; init; }
    public required string NormalizedModel { get; init; }
    public string? ColorName { get; init; }
    public required string NormalizedColor { get; init; }
    public string? StorageName { get; init; }
    public required string NormalizedStorage { get; init; }
    public required string ProductClassification { get; init; }
    public required string ImageUrl { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public required decimal DiscountAmount { get; init; }
    public required decimal OriginalPrice { get; init; }
    public required decimal FinalPrice { get; init; }
    public required int Stock { get; init; }
    public required int Sold { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }
    public string? DeletedBy { get; init; }
}
