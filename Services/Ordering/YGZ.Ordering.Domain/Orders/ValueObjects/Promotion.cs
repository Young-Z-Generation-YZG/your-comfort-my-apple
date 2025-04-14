

using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class Promotion : ValueObject
{
    public required string PromotionIdOrCode { get; set; }

    public required string PromotionEventType { get; set; }

    public string PromotionTitle { get; set; } = "UNKNOWN";

    public required string PromotionDiscountType { get; set; }

    public required decimal PromotionDiscountValue { get; set; }
    public required decimal PromotionDiscountUnitPrice { get; set; }
    public required int PromotionAppliedProductCount { get; set; }
    public required decimal PromotionFinalPrice { get; set; }

    public static Promotion Create(string promotionIdOrCode,
                                    string promotionEventType,
                                    string promotionTitle,
                                    string promotionDiscountType,
                                    decimal promotionDiscountValue,
                                    decimal promotionDiscountUnitPrice,
                                    int promotionAppliedProductCount,
                                    decimal promotionFinalPrice)
    {
        return new Promotion
        {
            PromotionIdOrCode = promotionIdOrCode,
            PromotionTitle = promotionTitle!,
            PromotionEventType = promotionEventType,
            PromotionDiscountType = promotionDiscountType,
            PromotionDiscountValue = promotionDiscountValue,
            PromotionDiscountUnitPrice = promotionDiscountUnitPrice,
            PromotionAppliedProductCount = promotionAppliedProductCount,
            PromotionFinalPrice = promotionFinalPrice
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return PromotionIdOrCode;
        yield return PromotionEventType;
        yield return PromotionTitle;
        yield return PromotionDiscountType;
        yield return PromotionDiscountValue;
        yield return PromotionDiscountUnitPrice;
        yield return PromotionAppliedProductCount;
        yield return PromotionFinalPrice;
    }
}
