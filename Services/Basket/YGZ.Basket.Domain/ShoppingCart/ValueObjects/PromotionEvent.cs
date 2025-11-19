using YGZ.BuildingBlocks.Messaging.IntegrationEvents.BasketService;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class PromotionEvent
{
    public required string PromotionId { get; init; }
    public required string PromotionType { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }

    public static PromotionEvent Create(string promotionId, string promotionType, EDiscountType discountType, decimal discountValue)
    {
        return new PromotionEvent
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
            PromotionType = EPromotionType.EVENT_ITEM.Name,
            DiscountType = DiscountType,
            DiscountValue = DiscountValue,
        };
    }

    public PromotionIntegrationEvent? ToPromotionIntegrationEvent()
    {
        return new PromotionIntegrationEvent
        {
            PromotionId = PromotionId,
            PromotionType = EPromotionType.EVENT_ITEM.Name,
            DiscountType = DiscountType,
            DiscountValue = DiscountValue,
        };
    }
}
