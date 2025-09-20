using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Domain.Event.Entities;

public class EventProductSKU : Entity<EventProductSKUId>, IAuditable, ISoftDelete
{
    public EventProductSKU(EventProductSKUId id) : base(id) { }

    public required EventId EventId { get; set; }
    public required string SKUId { get; set; }
    public required string ModelId { get; set; }
    public required string ModelName { get; set; }
    public required string StorageName { get; set; }
    public required string ColorHaxCode { get; set; }
    public required EProductType ProductType { get; set; }
    public required string ImageUrl { get; set; }
    public required EDiscountType DiscountType { get; set; }
    public required decimal DiscountValue { get; set; }
    public required decimal OriginalPrice { get; set; }
    public required int Stock { get; set; }
    public int Sold { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; private set; } = null;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; private set; } = null;


    public static EventProductSKU Create(EventProductSKUId id,
                                         EventId eventId,
                                         string skuId,
                                         string modelId,
                                         string modelName,
                                         string storageName,
                                         string colorHaxCode,
                                         EProductType productType,
                                         EDiscountType discountType,
                                         string imageUrl,
                                         decimal discountValue,
                                         decimal originalPrice,
                                         int stock)

    {
        return new EventProductSKU(id)
        {
            EventId = eventId,
            SKUId = skuId,
            ModelId = modelId,
            ModelName = modelName,
            StorageName = storageName,
            ColorHaxCode = colorHaxCode,
            ProductType = productType,
            ImageUrl = imageUrl,
            DiscountType = discountType,
            DiscountValue = discountValue,
            OriginalPrice = originalPrice,
            Stock = stock,
        };
    }
}
