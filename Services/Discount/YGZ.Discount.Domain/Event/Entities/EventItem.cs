using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.Event.Events;
using YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Domain.Event.Entities;

public class EventItem : Entity<EventItemId>, IAuditable, ISoftDelete
{
    public EventItem(EventItemId id) : base(id) { }

    public required EventId EventId { get; init; }
    public required string SkuId { get; init; }
    public required string TenantId { get; init; }
    public required string BranchId { get; init; }
    public required string ModelName { get; init; }
    public required string NormalizedModel { get; init; }
    public required string ColorName { get; init; }
    public required string NormalizedColor { get; init; }
    public required string ColorHaxCode { get; init; }
    public required string StorageName { get; init; }
    public required string NormalizedStorage { get; init; }
    public required EProductClassification ProductClassification { get; init; }
    public required string ImageUrl { get; init; }
    public required EDiscountType DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public required decimal OriginalPrice { get; init; }
    public required int Stock { get; init; }
    public int Sold { get; init; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; private set; } = null;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; private set; } = null;


    public static EventItem Create(EventItemId eventItemId,
                                   EventId eventId,
                                   string skuId,
                                   string tenantId,
                                   string branchId,
                                   string modelName,
                                   string normalizedModel,
                                   string colorName,
                                   string normalizedColor,
                                   string colorHaxCode,
                                   string storageName,
                                   string normalizedStorage,
                                   EProductClassification productClassification,
                                   EDiscountType discountType,
                                   string imageUrl,
                                   decimal discountValue,
                                   decimal originalPrice,
                                   int stock)
    {
        var eventItem = new EventItem(eventItemId)
        {
            EventId = eventId,
            SkuId = skuId,
            TenantId = tenantId,
            BranchId = branchId,
            ModelName = modelName,
            NormalizedModel = normalizedModel,
            ColorName = colorName,
            NormalizedColor = normalizedColor,
            ColorHaxCode = colorHaxCode,
            StorageName = storageName,
            NormalizedStorage = normalizedStorage,
            ProductClassification = productClassification,
            DiscountType = discountType,
            ImageUrl = imageUrl,
            DiscountValue = discountValue,
            OriginalPrice = originalPrice,
            Stock = stock
        };

        eventItem.AddDomainEvent(new EventItemCreatedDomainEvent(eventItem));


        return eventItem;
    }

    public EventItemResponse ToResponse()
    {
        return new EventItemResponse
        {
            Id = Id.Value.ToString()!,
            EventId = EventId.Value.ToString()!,
            ModelName = ModelName,
            NormalizedModel = NormalizedModel,
            ColorName = ColorName,
            NormalizedColor = NormalizedColor,
            ColorHexCode = ColorHaxCode,
            StorageName = StorageName,
            NormalizedStorage = NormalizedStorage,
            ProductClassification = ProductClassification.Name,
            ImageUrl = ImageUrl,
            DiscountType = DiscountType.Name,
            DiscountValue = DiscountValue,
            OriginalPrice = OriginalPrice,
            Stock = Stock,
            Sold = Sold,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            UpdatedBy = UpdatedBy,
            IsDeleted = IsDeleted,
            DeletedAt = DeletedAt,
            DeletedBy = DeletedBy
        };
    }
}
