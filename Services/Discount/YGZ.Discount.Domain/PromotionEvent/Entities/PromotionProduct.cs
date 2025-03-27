

using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

namespace YGZ.Discount.Domain.PromotionEvent.Entities;

public class PromotionProduct : Entity<ProductId>
{
    public PromotionProduct(ProductId id) : base(id) { }
    private PromotionProduct() : base(null!) { }

    required public string ProductSlug { get; set; }
    required public string ProductImage { get; set; } = default!;
    required public DiscountType DiscountType { get; set; }
    required public decimal DiscountValue { get; set; }
    required public PromotionGlobalId PromotionGlobalId { get; set; }

    public static PromotionProduct Create(ProductId id,
                                          string productSlug,
                                          string productImage,
                                          DiscountType discountType,
                                          decimal discountValue,
                                          PromotionGlobalId promotionGlobalId)
    {
        return new PromotionProduct(id)
        {
            ProductSlug = productSlug,
            ProductImage = productImage,
            DiscountType = discountType,
            DiscountValue = discountValue,
            PromotionGlobalId = promotionGlobalId
        };
    }
}
