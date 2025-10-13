using YGZ.BuildingBlocks.Messaging.IntegrationEvents.BasketService;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class PromotionEvent
{
    public string EventId { get; init; } = string.Empty;
    public string EventItemId { get; init; } = string.Empty;
    public decimal ProductUnitPrice { get; init; }
    public string DiscountType { get; init; } = string.Empty;
    public decimal DiscountValue { get; init; }
    public decimal DiscountAmount
    {
        get
        {
            if (DiscountType == EDiscountType.PERCENTAGE.Name)
            {
                return ProductUnitPrice * DiscountValue;
            }
            else
            {
                return DiscountValue;
            }
        }
    }

    public decimal FinalPrice
    {
        get
        {
            return ProductUnitPrice - DiscountAmount;
        }
    }

    public static PromotionEvent Create(string eventId, string eventItemId, decimal productUnitPrice, EDiscountType discountType, decimal discountValue)
    {
        return new PromotionEvent
        {
            EventId = eventId,
            EventItemId = eventItemId,
            DiscountType = discountType.Name,
            ProductUnitPrice = productUnitPrice,
            DiscountValue = discountValue,
        };
    }

    public PromotionResponse ToResponse()
    {
        return new PromotionResponse
        {
            PromotionType = EPromotionType.EVENT.Name,
            ProductUnitPrice = ProductUnitPrice,
            DiscountType = DiscountType,
            DiscountValue = DiscountValue,
            DiscountAmount = DiscountAmount,
            FinalPrice = FinalPrice,
        };
    }

    public PromotionIntegrationEvent? ToPromotionIntegrationEvent()
    {
        return new PromotionIntegrationEvent
        {
            PromotionIdOrCode = EventId,
            PromotionType = EPromotionType.EVENT.Name,
            DiscountType = DiscountType,
            DiscountValue = DiscountValue,
            DiscountAmount = DiscountAmount,
            FinalPrice = FinalPrice,
        };
    }
}
