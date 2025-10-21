using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Event.Entities;
using YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Infrastructure.Persistence.Seeds;

public static class SeedEventItemData
{
    public static IEnumerable<EventItem> EventItems
    {
        get
        {
            return new List<EventItem>
            {
                EventItem.Create(
                        eventItemId: EventItemId.Of("99a356c8-c026-4137-8820-394763f30521"),
                        eventId: EventId.Of("611db6eb-3d64-474e-9e23-3517ad0df6ec"),
                        skuId: "68f71b4a574ee8ac95e48e17",
                        tenantId: "664355f845e56534956be32b",
                        branchId: "664357a235e84033bbd0e6b6",
                        modelName: "iPhone 15",
                        normalizedModel: EIphoneModel.IPHONE_15.Name,
                        colorName: "Blue",
                        normalizedColor: EColor.BLUE.Name,
                        colorHaxCode: "#D5DDDF",
                        storageName: EStorage.STORAGE_256.Name,
                        normalizedStorage: EStorage.STORAGE_256.Name,
                        productClassification: EProductClassification.IPHONE,
                        discountType: EDiscountType.PERCENTAGE,
                        imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp",
                        discountValue: 10,
                        originalPrice: 100,
                        stock: 10
                    )
            };
        }
    }
}
