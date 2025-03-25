

using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

namespace YGZ.Discount.Domain.PromotionEvent.Entities;

public class PromotionCategory : Entity<CategoryId>
{
    public PromotionCategory(CategoryId id) : base(id) { }
    private PromotionCategory() : base(null!) { }

    public string CategoryName { get; set; } = default!;
    public string CategorySlug { get; set; } = default!;
    public decimal DiscountPercent { get; set; } = default!;
    required public PromotionGlobalId PromotionGlobalId { get; set; }

    public static PromotionCategory Create(
        CategoryId id,
        string categoryName,
        string categorySlug,
        decimal discountPercent,
        PromotionGlobalId promotionGlobalId
    )
    {
        return new PromotionCategory(id)
        {
            CategoryName = categoryName,
            CategorySlug = categorySlug,
            DiscountPercent = discountPercent,
            PromotionGlobalId = promotionGlobalId
        };
    }
}
