
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Baskets;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record GetBasketResponse()
{
    public required string UserEmail { get; init; }
    public required List<CartItemResponse> CartItems { get; init; }
    public required decimal TotalAmount { get; init; }
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
    public required decimal SubTotalAmount { get; init; }
    public PromotionResponse? Promotion { get; init; }
    public required int Index { get; init; }
}

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record PromotionResponse()
{
    public required string PromotionId { get; init; }
    public required string PromotionType { get; init; }
    public required decimal ProductUnitPrice { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public required decimal DiscountAmount { get; init; }
    public required decimal FinalPrice { get; init; }
}


