using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Domain.Event.Entities;

public class EventItem : Entity<EventItemId>, IAuditable, ISoftDelete
{
    public EventItem(EventItemId id) : base(id) { }

    public required EventId EventId { get; set; }
    public required string ModelName { get; set; }
    public required string NormalizedModel { get; set; }
    public required string ColorName { get; set; }
    public required string NormalizedColor { get; set; }
    public required string ColorHaxCode { get; set; }
    public required string StorageName { get; set; }
    public required string NormalizedStorage { get; set; }
    public required EProductClassification CategoryType { get; set; }
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


    public static EventItem Create(EventId eventId,
                                   string modelName,
                                   string normalizedModel,
                                   string colorName,
                                   string normalizedColor,
                                   string colorHaxCode,
                                   string storageName,
                                   string normalizedStorage,
                                   EProductClassification categoryType,
                                   EDiscountType discountType,
                                   string imageUrl,
                                   decimal discountValue,
                                   decimal originalPrice,
                                   int stock)
    {

        return new EventItem(EventItemId.Create())
        {
            EventId = eventId,
            ModelName = modelName,
            NormalizedModel = normalizedModel,
            ColorName = colorName,
            NormalizedColor = normalizedColor,
            ColorHaxCode = colorHaxCode,
            StorageName = storageName,
            NormalizedStorage = normalizedStorage,
            CategoryType = categoryType,
            DiscountType = discountType,
            ImageUrl = imageUrl,
            DiscountValue = discountValue,
            OriginalPrice = originalPrice,
            Stock = stock
        };
    }
}
