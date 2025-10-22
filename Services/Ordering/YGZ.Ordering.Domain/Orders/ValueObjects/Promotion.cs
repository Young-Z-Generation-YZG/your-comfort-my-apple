using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;
using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class Promotion : ValueObject
{
    public string PromotionId { get; init; }
    public string PromotionType { get; init; }
    public decimal ProductUnitPrice { get; init; }
    public string DiscountType { get; init; }
    public decimal DiscountValue { get; init; }
    public decimal DiscountAmount { get; init; }

    public decimal FinalPrice
    {
        get
        {
            if (DiscountType == EDiscountType.PERCENTAGE.Name)
            {
                return ProductUnitPrice - (ProductUnitPrice * DiscountValue);
            }
            else
            {
                return ProductUnitPrice - DiscountValue;
            }
        }
    }

    private Promotion(string promotionId,
                      string promotionType,
                      decimal productUnitPrice,
                      string discountType,
                      decimal discountValue,
                      decimal discountAmount)
    {
        PromotionId = promotionId;
        PromotionType = promotionType;
        ProductUnitPrice = productUnitPrice;
        DiscountType = discountType;
        DiscountValue = discountValue;
        DiscountAmount = discountAmount;
    }

    public static Promotion Create(string promotionId,
                                   string promotionType,
                                   decimal productUnitPrice,
                                   string discountType,
                                   decimal discountValue,
                                   decimal discountAmount)
    {
        return new Promotion(promotionId,
                             promotionType,
                             productUnitPrice,
                             discountType,
                             discountValue,
                             discountAmount);
    }

    public static Promotion Of(
        string promotionId,
        string promotionType,
        decimal productUnitPrice,
        string discountType,
        decimal discountValue,
        decimal discountAmount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(promotionId);
        ArgumentException.ThrowIfNullOrWhiteSpace(promotionType);
        ArgumentException.ThrowIfNullOrWhiteSpace(discountType);

        return new Promotion(promotionId,
                             promotionType,
                             productUnitPrice,
                             discountType,
                             discountValue,
                             discountAmount);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return PromotionId;
        yield return PromotionType;
        yield return DiscountType;
        yield return DiscountValue;
        yield return DiscountAmount;
    }
}
