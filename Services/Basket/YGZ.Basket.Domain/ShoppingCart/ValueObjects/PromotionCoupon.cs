using YGZ.BuildingBlocks.Messaging.IntegrationEvents.BasketService;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class PromotionCoupon
{
    public required string PromotionId { get; init; }
    public required string PromotionType { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }

    public static PromotionCoupon Create(string promotionId, string promotionType, EDiscountType discountType, decimal discountValue)
    {
        return new PromotionCoupon
        {
            PromotionId = promotionId,
            PromotionType = promotionType,
            DiscountType = discountType.Name,
            DiscountValue = discountValue,
        };
    }

    public PromotionResponse ToResponse()
    {
        return new PromotionResponse
        {
            PromotionId = PromotionId,
            PromotionType = EPromotionType.COUPON.Name,
            DiscountType = DiscountType,
            DiscountValue = DiscountValue,
        };
    }

    public PromotionIntegrationEvent? ToPromotionIntegrationEvent()
    {
        return new PromotionIntegrationEvent
        {
            PromotionId = PromotionId,
            PromotionType = PromotionType,
            DiscountType = DiscountType,
            DiscountValue = DiscountValue,
        };
    }
}
