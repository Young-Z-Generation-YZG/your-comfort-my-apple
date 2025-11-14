using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Discounts;


[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record CouponResponse
{
    public required string Id { get; init; }
    public string? UserId { get; init; }
    public required string Title { get; init; }
    public required string Code { get; init; }
    public required string Description { get; init; }
    public required string ProductClassification { get; init; }
    public required string PromotionType { get; init; }
    public required string DiscountState { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public decimal? MaxDiscountAmount { get; init; }
    public required int AvailableQuantity { get; init; }
    public required int Stock { get; init; }
}