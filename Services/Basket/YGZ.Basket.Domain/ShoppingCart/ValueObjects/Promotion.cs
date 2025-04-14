

namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class Promotion
{
    public required string PromotionIdOrCode { get; set; }

    public required string PromotionEventType { get; set; }

    public string PromotionTitle { get; set; } = "UNKNOWN";

    public required string PromotionDiscountType { get; set; }

    public required decimal? PromotionDiscountValue { get; set; } = 0;
    public required decimal? PromotionDiscountUnitPrice { get; set; } = 0;
    public required int? PromotionAppliedProductCount { get; set; } = 0;
    public required decimal? PromotionFinalPrice { get; set; } = 0;

    public static Promotion Create(string promotionIdOrCode,
                                   string promotionEventType,
                                   string? promotionTitle,
                                   string promotionDiscountType,
                                   decimal? promotionDiscountValue,
                                   decimal? promotionDiscountUnitPrice,
                                   int? promotionAppliedProductCount,
                                   decimal? promotionFinalPrice)
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
}