
using Ardalis.SmartEnum;

namespace YGZ.Basket.Domain.Core.Enums;

public class PromotionEvent : SmartEnum<PromotionEvent>
{
    public PromotionEvent(string name, int value) : base(name, value) { }

    public static readonly PromotionEvent PROMOTION_UNKNOWN = new("PROMOTION_UNKNOWN", 0);
    public static readonly PromotionEvent PROMOTION_COUPON = new("PROMOTION_COUPON", 1);
    public static readonly PromotionEvent PROMOTION_ITEM = new("PROMOTION_ITEM", 2);
    public static readonly PromotionEvent PROMOTION_EVENT = new("PROMOTION_EVENT", 3);
}
