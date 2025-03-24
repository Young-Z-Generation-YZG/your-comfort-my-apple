
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Application.Abstractions;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;
using YGZ.Discount.Domain.PromotionItem.ValueObjects;

namespace YGZ.Discount.Domain.PromotionItem;

public class PromotionItem : AggregateRoot<PromotionItemId>, IAuditable, ISoftDelete
{
    public PromotionItem(PromotionItemId id) : base(id)
    {
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public NameTag NameTag { get; set; }
    public DiscountState State { get; set; } = DiscountState.INACTIVE;
    public DiscountType Type { get; set; } = DiscountType.PERCENT;
    public double DiscountValue { get; set; } = 0;
    public DateTime? ValidFrom { get; set; } = null;
    public DateTime? ValidTo { get; set; } = null;
    public int AvailableQuantity { get; set; }
    public ProductId ProductId { get; set; }
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
                                       DiscountType type,
                                       double discountValue,
                                       NameTag nameTag,
                                       DateTime? validFrom,
                                       DateTime? validTo,
                                       int availableQuantity,
                                       ProductId productId,
                                       string productSlug)
    {
        return new PromotionItem(promotionItemId)
        {
            Title = title,
            Description = description,
            Type = type,
            DiscountValue = discountValue,
            NameTag = nameTag,
            ValidFrom = validFrom,
            ValidTo = validTo,
            AvailableQuantity = availableQuantity,
            ProductId = productId,
            ProductSlug = productSlug
        };
    }

}
