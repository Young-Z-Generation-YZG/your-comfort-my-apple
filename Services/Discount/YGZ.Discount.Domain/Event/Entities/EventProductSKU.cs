
using YGZ.Discount.Application.Abstractions;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Domain.Event.Entities;

public class EventProductSKU : Entity<EventProducSKUId>, IAuditable, ISoftDelete
{
    public EventProductSKU(EventProducSKUId id) : base(id) { }

    public required EventId EventId { get; set; }
    public required string SKUId { get; set; }
    public required EProductType ProductType { get; set; }
    public int AvailableQuantity { get; set; } = 0;
    public required EDiscountType DiscountType { get; set; }
    public required decimal DiscountValue { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; private set; } = null;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; private set; } = null;


    public static EventProductSKU Create(EventProducSKUId id,
                                         EventId eventId,
                                         string skuId,
                                         EProductType productType,
                                         int availableQuantity,
                                         EDiscountType discountType,
                                         decimal discountValue)
    {
        return new EventProductSKU(id)
        {
            EventId = eventId,
            SKUId = skuId,
            ProductType = productType,
            AvailableQuantity = availableQuantity,
            DiscountType = discountType,
            DiscountValue = discountValue
        };
    }
}
