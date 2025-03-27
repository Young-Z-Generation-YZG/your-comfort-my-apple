
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Application.Abstractions;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;
using YGZ.Discount.Domain.PromotionItem.ValueObjects;

namespace YGZ.Discount.Domain.PromotionItem;

public class PromotionItem : AggregateRoot<ProductId>, IAuditable, ISoftDelete
{
    public PromotionItem(ProductId id) : base(id)
    {
    }

    required public string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    required public ProductNameTag ProductNameTag { get; set; }
    required public PromotionEventType PromotionEventType { get; set; } = PromotionEventType.PROMOTION_ITEM;
    public DiscountState DiscountState { get; set; } = DiscountState.INACTIVE;
    required public DiscountType DiscountType { get; set; } = DiscountType.PERCENTAGE;
    public EndDiscountType EndDiscountType { get; set; } = EndDiscountType.BY_END_DATE;
    required public decimal DiscountValue { get; set; } = 0;
    public DateTime? ValidFrom { get; set; } = null;
    public DateTime? ValidTo { get; set; } = null;
    public int? AvailableQuantity { get; set; } = null;
    required public string ProductImage { get; set; }
    required public string ProductSlug { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; set; } = null;

    public static PromotionItem Create(ProductId productId,
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
                                       string productImage,
                                       string productSlug)
    {
        return new PromotionItem(productId)
        {
            Title = title,
            Description = description,
            PromotionEventType = PromotionEventType.PROMOTION_ITEM,
            DiscountState = discountState,
            DiscountType = discountType,
            DiscountValue = discountValue,
            ProductNameTag = nameTag,
            ValidFrom = validFrom,
            ValidTo = validTo,
            AvailableQuantity = availableQuantity,
            ProductSlug = productSlug,
            ProductImage = productImage
        };
    }

}
