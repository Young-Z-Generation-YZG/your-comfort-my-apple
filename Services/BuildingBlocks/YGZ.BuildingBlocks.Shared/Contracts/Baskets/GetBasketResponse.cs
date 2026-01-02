
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Baskets;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record CouponApplied()
{
    public string? ErrorMessage { get; init; }
    public string? Title { get; init; }
    public string? DiscountType { get; init; }
    public double? DiscountValue { get; init; }
    public double? MaxDiscountAmount { get; init; }
    public string? Description { get; init; }
    public DateTime? ExpiredDate { get; init; }
}

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record GetBasketResponse()
{
    public required string UserEmail { get; init; }
    public required List<CartItemResponse> CartItems { get; init; }
    public decimal SubTotalAmount { get; init; }
    public string? PromotionId { get; init; }
    public string? PromotionType { get; init; }
    public string? DiscountType { get; init; }
    public decimal? DiscountValue { get; init; }
    public decimal? DiscountAmount { get; init; }
    public decimal? MaxDiscountAmount { get; init; }
    public CouponApplied? CouponApplied { get; init; }
    public decimal TotalAmount { get; init; }
}

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record CartItemResponse()
{
    public required bool IsSelected { get; init; }
    public required string ModelId { get; init; }
    public required string SkuId { get; init; }
    public required string ProductName { get; init; }
    public required ColorResponse Color { get; init; }
    public required ModelResponse Model { get; init; }
    public required StorageResponse Storage { get; init; }
    public required string DisplayImageUrl { get; init; }
    public required decimal UnitPrice { get; init; }
    public required int Quantity { get; init; }
    public required int QuantityRemain { get; init; }
    public required bool IsOutOfStock { get; init; }
    public required decimal SubTotalAmount { get; init; }
    public PromotionResponse? Promotion { get; init; }
    public decimal? DiscountAmount { get; init; }
    public required decimal TotalAmount { get; init; }
    public required int Index { get; init; }
}

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record PromotionResponse()
{
    public required string PromotionId { get; init; }
    public required string PromotionType { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
}


