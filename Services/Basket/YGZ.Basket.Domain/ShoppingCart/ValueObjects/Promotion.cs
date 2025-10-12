using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class Promotion
{
    public required string PromotionType { get; init; }
    public PromotionCoupon? PromotionCoupon { get; init; }
    public PromotionEvent? PromotionEvent { get; init; }

    public static Promotion Create(string promotionType,
                                   PromotionCoupon? PromotionCoupon,
                                   PromotionEvent? PromotionEvent)
    {
        EPromotionType.TryFromName(SnakeCaseSerializer.Serialize(promotionType).ToUpper(), out var promotionTypeEnum);

        if (promotionTypeEnum is null)
        {
            throw new ArgumentException("Invalid promotion type", promotionType);
        }

        return new Promotion
        {
            PromotionType = promotionTypeEnum.Name,
            PromotionCoupon = PromotionCoupon,
            PromotionEvent = PromotionEvent,
        };
    }

    public PromotionResponse? ToResponse()
    {
        if(PromotionCoupon is not null && PromotionEvent is not null) {
            throw new Exception("Promotion cannot be both coupon and event");
        }

        if(PromotionCoupon is not null) {
            return PromotionCoupon.ToResponse();
        }

        if(PromotionEvent is not null) {
            return PromotionEvent.ToResponse();
        }

        return null;
    }
}