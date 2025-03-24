

using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

namespace YGZ.Discount.Domain.PromotionEvent.Entities;

public class PromotionProduct : Entity<ProductId>
{
    public PromotionProduct(ProductId id) : base(id) { }
    private PromotionProduct() : base(null!) { }

    public string ProductColorName { get; set; } = default!;
    public int ProductStorage { get; set; } = default!;
    public string ProductSlug { get; set; } = default!;
    public string ProductImage { get; set; } = default!;
    public decimal DiscountPercentage { get; set; } = default!;
    required public PromotionGlobalId PromotionGlobalId { get; set; }

    public static PromotionProduct Create(ProductId id,
                                          string productColorName,
                                          int productStorage,
                                          string productSlug,
                                          string productImage,
                                          decimal discountPercentage,
                                          PromotionGlobalId promotionGlobalId)
    {
        return new PromotionProduct(id)
        {
            ProductColorName = productColorName,
            ProductStorage = productStorage,
            ProductSlug = productSlug,
            ProductImage = productImage,
            DiscountPercentage = discountPercentage,
            PromotionGlobalId = promotionGlobalId
        };
    }
}
