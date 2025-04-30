

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Discounts;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record PromotionCouponResponse
{
    public string PromotionCouponId { get; init; }
    public string PromotionCouponTitle { get; init; }
    public string PromotionCouponCode { get; init; }
    public string PromotionCouponDescription { get; init; }
    public string PromotionCouponProductNameTag { get; init; }
    public string PromotionCouponPromotionEventType { get; init; }
    public string PromotionCouponDiscountState { get; init; }
    public string PromotionCouponDiscountType { get; init; }
    public decimal PromotionCouponDiscountValue { get; init; }
    public decimal? PromotionCouponMaxDiscountAmount { get; init; }
    public DateTime? PromotionCouponValidFrom { get; init; } = null;
    public DateTime? PromotionCouponValidTo { get; init; } = null;
    public int PromotionCouponAvailableQuantity { get; init; }
}