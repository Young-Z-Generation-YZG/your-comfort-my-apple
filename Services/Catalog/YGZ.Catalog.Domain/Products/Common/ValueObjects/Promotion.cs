using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class Promotion : ValueObject
{
    [BsonElement("promotion_id")]
    public string PromotionId { get; init; }

    [BsonElement("promotion_type")]
    public string PromotionType { get; init; }

    [BsonElement("product_unit_price")]
    public decimal ProductUnitPrice { get; init; }

    [BsonElement("discount_type")]
    public string DiscountType { get; init; }

    [BsonElement("discount_value")]
    public decimal DiscountValue { get; init; }

    [BsonElement("discount_amount")]
    public decimal DiscountAmount { get; init; }

    [BsonElement("final_price")]
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
    
    private Promotion(string promotionId, string promotionType, decimal productUnitPrice, string discountType, decimal discountValue, decimal discountAmount)
    {
        PromotionId = promotionId;
        PromotionType = promotionType;
        ProductUnitPrice = productUnitPrice;
        DiscountType = discountType;
        DiscountValue = discountValue;
        DiscountAmount = discountAmount;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return PromotionId;
        yield return PromotionType;
        yield return ProductUnitPrice;
        yield return DiscountType;
        yield return DiscountValue;
        yield return DiscountAmount;
        yield return FinalPrice;
    }

    public static Promotion Create(string promotionId, string promotionType, decimal productUnitPrice, string discountType, decimal discountValue, decimal discountAmount)
    {
        return new Promotion(promotionId, promotionType, productUnitPrice, discountType, discountValue, discountAmount);
    }

    public PromotionResponse ToResponse()
    {
        return new PromotionResponse
        {
            PromotionId = PromotionId,
            PromotionType = PromotionType,
            DiscountType = DiscountType,
            DiscountValue = DiscountValue,
        };
    }
}