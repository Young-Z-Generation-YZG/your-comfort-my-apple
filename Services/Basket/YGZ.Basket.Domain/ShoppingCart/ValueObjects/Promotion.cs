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
        EPromotionType.TryFromName(SnakeCaseSerializer.Serialize(promotionType).ToUpper(), out var promtoionTypeEnum);

        if (promtoionTypeEnum is null)
        {
            throw new ArgumentException("Invalid promotion type", promotionType);
        }

        return new Promotion
        {
            PromotionType = promtoionTypeEnum.Name,
            PromotionCoupon = PromotionCoupon,
            PromotionEvent = PromotionEvent,
        };
    }
}