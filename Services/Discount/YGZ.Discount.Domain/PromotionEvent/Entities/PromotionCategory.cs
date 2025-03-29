

using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

namespace YGZ.Discount.Domain.PromotionEvent.Entities;

public class PromotionCategory : Entity<CategoryId>
{
    public PromotionCategory(CategoryId id) : base(id) { }
    private PromotionCategory() : base(null!) { }

    required public string CategoryName { get; set; }
    required public string CategorySlug { get; set; }
    required public DiscountType DiscountType { get; set; }
    required public decimal DiscountValue { get; set; }
    required public PromotionGlobalId PromotionGlobalId { get; set; }

    public static PromotionCategory Create(
        CategoryId id,
        string categoryName,
        string categorySlug,
        DiscountType discountType,
        decimal discountValue,
        PromotionGlobalId promotionGlobalId
    )
    {
        return new PromotionCategory(id)
        {
            CategoryName = categoryName,
            CategorySlug = categorySlug,
            DiscountType = discountType,
            DiscountValue = discountValue,
            PromotionGlobalId = promotionGlobalId
        };
    }
}
