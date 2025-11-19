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
    public string? ModelName { get; private set; }
    public required string NormalizedModel { get; init; }
    public string? ColorName { get; private set; }
    public required string NormalizedColor { get; init; }
    public required string ColorHexCode { get; init; }
    public string? StorageName { get; private set; }
    public required string NormalizedStorage { get; init; }
    public required EProductClassification ProductClassification { get; init; }
    public required string ImageUrl { get; init; }
    public required EDiscountType DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public decimal DiscountAmount { get; set; }
    public required decimal OriginalPrice { get; init; }
    public decimal FinalPrice { get; set; }
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
                                   EIphoneModel iphoneModelEnum,
                                   EColor colorEnum,
                                   EStorage storageEnum,
                                   EProductClassification productClassification,
                                   EDiscountType discountType,
                                   string colorHexCode,
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
            ModelName = FormatModelName(iphoneModelEnum.Name),
            NormalizedModel = iphoneModelEnum.Name,
            ColorName = FormatColorName(colorEnum.Name),
            NormalizedColor = colorEnum.Name,
            ColorHexCode = colorHexCode,
            StorageName = FormatStorageName(storageEnum.Name),
            NormalizedStorage = storageEnum.Name,
            ProductClassification = productClassification,
            DiscountType = discountType,
            ImageUrl = imageUrl,
            DiscountValue = discountValue,
            OriginalPrice = originalPrice,
            Stock = stock,
            DiscountAmount = CalculateDiscountAmount(originalPrice, discountType, discountValue),
            FinalPrice = CalculateFinalPrice(originalPrice, discountType, discountValue)
        };

        eventItem.AddDomainEvent(new EventItemCreatedDomainEvent(eventItem));

        return eventItem;
    }

    private static decimal CalculateDiscountAmount(decimal originalPrice, EDiscountType discountType, decimal discountValue)
    {
        var amount = discountType switch
        {
            var type when type == EDiscountType.PERCENTAGE => originalPrice * (discountValue / 100m),
            var type when type == EDiscountType.FIXED_AMOUNT => discountValue,
            _ => 0m
        };

        amount = Math.Clamp(amount, 0, originalPrice);

        return decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
    }

    private static decimal CalculateFinalPrice(decimal originalPrice, EDiscountType discountType, decimal discountValue)
    {
        var discountAmount = CalculateDiscountAmount(originalPrice, discountType, discountValue);
        return Math.Max(originalPrice - discountAmount, 0);
    }

    private static string? FormatModelName(string normalizedModel)
    {
        if (string.IsNullOrWhiteSpace(normalizedModel))
        {
            return null;
        }

        var parts = normalizedModel.Split('_', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0)
        {
            return normalizedModel;
        }

        var formattedParts = new List<string>(parts.Length);

        for (var i = 0; i < parts.Length; i++)
        {
            if (i == 0 && parts[i].Equals("IPHONE", StringComparison.OrdinalIgnoreCase))
            {
                formattedParts.Add("iPhone");
                continue;
            }

            var lower = parts[i].ToLowerInvariant();
            formattedParts.Add(char.ToUpperInvariant(lower[0]) + lower[1..]);
        }

        return string.Join(' ', formattedParts);
    }

    private static string? FormatColorName(string normalizedColor)
    {
        if (string.IsNullOrWhiteSpace(normalizedColor))
        {
            return null;
        }

        var lower = normalizedColor.ToLowerInvariant();
        return char.ToUpperInvariant(lower[0]) + lower[1..];
    }

    private static string? FormatStorageName(string normalizedStorage)
    {
        return string.IsNullOrWhiteSpace(normalizedStorage) ? null : normalizedStorage;
    }

    public EventItemResponse ToResponse()
    {
        return new EventItemResponse
        {
            Id = Id.Value.ToString()!,
            EventId = EventId.Value.ToString()!,
            SkuId = SkuId,
            TenantId = TenantId,
            BranchId = BranchId,
            ModelName = ModelName,
            NormalizedModel = NormalizedModel,
            ColorName = ColorName,
            NormalizedColor = NormalizedColor,
            StorageName = StorageName,
            NormalizedStorage = NormalizedStorage,
            ProductClassification = ProductClassification.Name,
            ImageUrl = ImageUrl,
            DiscountType = DiscountType.Name,
            DiscountValue = DiscountValue,
            DiscountAmount = DiscountAmount,
            OriginalPrice = OriginalPrice,
            FinalPrice = FinalPrice,
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
