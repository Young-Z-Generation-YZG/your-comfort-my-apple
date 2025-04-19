

using System.ComponentModel.DataAnnotations;
using YGZ.Discount.Application.Abstractions;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

namespace YGZ.Discount.Domain.PromotionEvent;

public class PromotionEvent : AggregateRoot<PromotionEventId>, IAuditable, ISoftDelete
{
    public PromotionEvent(PromotionEventId id) : base(id) { }
    private PromotionEvent() : base(null!) { }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public PromotionEventType PromotionEventType { get; set; } = PromotionEventType.PROMOTION_EVENT;
    public DiscountState DiscountState { get; set; } = DiscountState.INACTIVE;
    public DateTime? ValidFrom { get; set; } = null;
    public DateTime? ValidTo { get; set; } = null;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; set; } = null;

    public static PromotionEvent Create(PromotionEventId id,
                                        string title,
                                        string description,
                                        DiscountState discountState,
                                        DateTime? validFrom,
                                        DateTime? validTo)
    {
        return new PromotionEvent(id)
        {
            Title = title,
            Description = description,
            DiscountState = discountState,
            ValidFrom = validFrom,
            ValidTo = validTo
        };
    }
}
