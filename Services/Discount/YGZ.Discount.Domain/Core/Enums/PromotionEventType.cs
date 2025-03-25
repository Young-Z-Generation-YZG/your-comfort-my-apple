
using Ardalis.SmartEnum;

namespace YGZ.Discount.Domain.Core.Enums;

public class PromotionEventType : SmartEnum<PromotionEventType>
{
    public PromotionEventType(string name, int value) : base(name, value) { }

    public static readonly PromotionEventType PROMOTION_COUPON = new("PROMOTION_COUPON", 1);
    public static readonly PromotionEventType PROMOTION_ITEM = new("PROMOTION_ITEM", 2);
    public static readonly PromotionEventType PROMOTION_EVENT = new("PROMOTION_EVENT", 3);
}
