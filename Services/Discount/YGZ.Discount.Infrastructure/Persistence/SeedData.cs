

using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;
using YGZ.Discount.Domain.PromotionEvent;
using YGZ.Discount.Domain.PromotionEvent.Entities;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;
using YGZ.Discount.Domain.PromotionItem;
using YGZ.Discount.Domain.PromotionItem.ValueObjects;

namespace YGZ.Discount.Infrastructure.Persistence;

public static class SeedData
{
    public static IEnumerable<PromotionEvent> PromotionEvents => new List<PromotionEvent>()
    {
        PromotionEvent.Create(id: PromotionEventId.Of("f55f322f-6406-4dfa-b2ea-2777f7813e70"),
            title: "Black Friday",
            description: "Sale all item in shop with special price",
            discountState: DiscountState.ACTIVE,
            validFrom: new DateTime(2025, 3, 25).ToUniversalTime(),
            validTo: new DateTime(2025, 3, 26).ToUniversalTime())
    };

    public static IEnumerable<PromotionGlobal> PromotionGlobals => new List<PromotionGlobal>()
    {
        PromotionGlobal.Create(id: PromotionGlobalId.Of("61cd23de-169e-4beb-a890-8dbb91ccca57"),
            title: "Black Friday for products",
            description: "Black Friday for specific products",
            promotionGlobalType: PromotionGlobalType.PRODUCTS,
            promotionEventId: PromotionEventId.Of("f55f322f-6406-4dfa-b2ea-2777f7813e70")),

        PromotionGlobal.Create(id: PromotionGlobalId.Of("2780383d-cef5-4111-84f0-7798261aa595"),
            title: "Black Friday for categories",
            description: "Black Friday for specific categories",
            promotionGlobalType: PromotionGlobalType.CATEGORIES,
            promotionEventId: PromotionEventId.Of("f55f322f-6406-4dfa-b2ea-2777f7813e70"))
    };

    public static IEnumerable<PromotionProduct> PromotionProducts => new List<PromotionProduct>()
    {
        PromotionProduct.Create(id: ProductId.Of("iphone-16-128gb"),
            productSlug: "iphone-16-128gb",
            productImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-1-202409_GEO_US",
            discountType: DiscountType.PERCENTAGE,
            discountValue: (decimal)0.05,
            promotionGlobalId: PromotionGlobalId.Of("61cd23de-169e-4beb-a890-8dbb91ccca57")),

        PromotionProduct.Create(id: ProductId.Of("iphone-16-256gb"),
            productSlug: "iphone-16-256gb",
            productImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-1-202409_GEO_US",
            discountType: DiscountType.PERCENTAGE,
            discountValue: (decimal)0.05,
            promotionGlobalId: PromotionGlobalId.Of("61cd23de-169e-4beb-a890-8dbb91ccca57"))
    };

    public static IEnumerable<PromotionCategory> promotionCategories => new List<PromotionCategory>()
    {
        PromotionCategory.Create(id: CategoryId.Of("67346f7549189f7314e4ef0c"),
            categoryName: "iPhone 16",
            categorySlug: "iphone-16",
            discountType: DiscountType.PERCENTAGE,
            discountValue: (decimal)0.05,
            promotionGlobalId: PromotionGlobalId.Of("2780383d-cef5-4111-84f0-7798261aa595"))
    };

    public static IEnumerable<PromotionItem> PromotionItems => new List<PromotionItem>()
    {
        PromotionItem.Create(
                productId: ProductId.Of("67cbcff3cb422bbaf809c5a9"),
                title: "Discount for iPhone 16 128GB",
                description: "Discount only for iPhone 16 128GB",
                discountState: DiscountState.ACTIVE,
                discountType: DiscountType.PERCENTAGE,
                endDiscountType: EndDiscountType.BY_END_DATE,
                discountValue: (decimal)0.05,
                nameTag: ProductNameTag.IPHONE,
                validFrom: new DateTime(2025, 3, 25).ToUniversalTime(),
                validTo: new DateTime(2025, 4, 25).ToUniversalTime(),
                availableQuantity: null,
                productImage: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-1-202409_GEO_US",
                productSlug: "iphone-16-128gb"
        )
    };

    public static IEnumerable<Coupon> Coupons => new List<Coupon>()
    {
        Coupon.Create(
                couponId: CouponId.Of("f55f322f-6406-4dfa-b2ea-2777f7813e70"),
                code: Code.Of("APRIL2025"),
                title: "April 2025",
                description: "Discount for April 2025",
                nameTag: ProductNameTag.IPHONE,
                promotionEventType: PromotionEventType.PROMOTION_COUPON,
                discountState: DiscountState.ACTIVE,
                discountType: DiscountType.PERCENTAGE,
                discountValue: (decimal)0.05,
                maxDiscountAmount: null,
                validFrom: new DateTime(2025, 4, 1).ToUniversalTime(),
                validTo: new DateTime(2025, 4, 30).ToUniversalTime(),
                availableQuantity: 100
            )
    };
}
