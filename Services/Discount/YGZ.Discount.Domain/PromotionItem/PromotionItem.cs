
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Application.Abstractions;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.PromotionItem.ValueObjects;

namespace YGZ.Discount.Domain.PromotionItem;

public class PromotionItem : AggregateRoot<PromotionItemId>, IAuditable, ISoftDelete
{
    public PromotionItem(PromotionItemId id) : base(id)
    {
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public ProductNameTag ProductNameTag { get; set; }
    public PromotionEventType PromotionEventType { get; set; } = PromotionEventType.PROMOTION_ITEM;
    public DiscountState DiscountState { get; set; } = DiscountState.INACTIVE;
    public DiscountType DiscountType { get; set; } = DiscountType.PERCENTAGE;
    public EndDiscountType EndDiscountType { get; set; } = EndDiscountType.BY_END_DATE;
    public decimal DiscountValue { get; set; } = 0;
    public DateTime? ValidFrom { get; set; } = null;
    public DateTime? ValidTo { get; set; } = null;
    public int? AvailableQuantity { get; set; } = null;
    public string ProductModel { get; set; }
    public int ProductStorage { get; set; }
    public string ProductSlug { get; set; }
    public string ProductImage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; set; } = null;

    public static PromotionItem Create(PromotionItemId promotionItemId,
                                       string title,
                                       string description,
                                       DiscountState discountState,
                                       DiscountType discountType,
                                       EndDiscountType endDiscountType,
                                       decimal discountValue,
                                       ProductNameTag nameTag,
                                       DateTime? validFrom,
                                       DateTime? validTo,
                                       int? availableQuantity,
                                       string productModel,
                                       int productStorage,
                                       string productSlug,
                                       string productImage)
    {
        return new PromotionItem(promotionItemId)
        {
            Title = title,
            Description = description,
            DiscountState = discountState,
            DiscountType = discountType,
            DiscountValue = discountValue,
            ProductNameTag = nameTag,
            ValidFrom = validFrom,
            ValidTo = validTo,
            AvailableQuantity = availableQuantity,
            ProductModel = productModel,
            ProductStorage = productStorage,
            ProductSlug = productSlug,
            ProductImage = productImage
        };
    }

}
