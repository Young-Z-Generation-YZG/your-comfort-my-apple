

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Ordering;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record OrderItemResponse()
{
    public required string OrderItemId { get; init; }
    public required string OrderId { get; init; }
    public string? TenantId { get; init; }
    public string? BranchId { get; init; }
    public string? SkuId { get; init; }
    public required string ModelId { get; init; }
    public required string ModelName { get; init; }
    public required string ColorName { get; init; }
    public required string StorageName { get; init; }
    public required decimal UnitPrice { get; init; }
    public required string DisplayImageUrl { get; init; }
    public required string ModelSlug { get; init; }
    public required int Quantity { get; init; }
    public bool IsReviewed { get; init; } = false;
        public string? PromotionId { get; init; }
    public string? PromotionType { get; init; }
    public string? DiscountType { get; init; }
    public decimal? DiscountValue { get; init; }
    public decimal? DiscountAmount { get; init; }
    public decimal SubTotalAmount {get; init;}
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    public string? UpdatedBy { get; init; } = null;
    public bool IsDeleted { get; init; } = false;
    public DateTime? DeletedAt { get; init; } = null;
    public string? DeletedBy { get; init; } = null;
}

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record PromotionResponse()
{
    public required string PromotionId { get; init; }
    public required string PromotionType { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public required decimal DiscountAmount { get; init; }
    public required decimal FinalPrice { get; init; }
}