using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

namespace YGZ.Discount.Domain.PromotionEvent.Entities;

public class PromotionGlobal : Entity<PromotionGlobalId>, IAuditable, ISoftDelete
{
    public PromotionGlobal(PromotionGlobalId id) : base(id) { }
    private PromotionGlobal() : base(null!) { }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    required public PromotionGlobalType PromotionGlobalType { get; set; }
    required public PromotionEventId PromotionEventId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; set; } = null;
    public PromotionEvent PromotionEvent { get; set; } = null!; // Navigation property

    public string? UpdatedBy => throw new NotImplementedException();

    public static PromotionGlobal Create(PromotionGlobalId id,
                                         string title,
                                         string description,
                                         PromotionGlobalType promotionGlobalType,
                                         PromotionEventId promotionEventId)
    {
        return new PromotionGlobal(id)
        {
            Title = title,
            Description = description,
            PromotionGlobalType = promotionGlobalType,
            PromotionEventId = promotionEventId
        };
    }
}
