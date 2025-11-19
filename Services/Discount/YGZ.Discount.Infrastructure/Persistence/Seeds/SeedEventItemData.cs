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
                    eventItemId: EventItemId.Create(),
                    eventId: EventId.Of("611db6eb-3d64-474e-9e23-3517ad0df6ec"),
                    skuId: "690f4601e2295b9f94f23f60",
                    tenantId: "664355f845e56534956be32b",
                    branchId: "664357a235e84033bbd0e6b6",
                    iphoneModelEnum: EIphoneModel.IPHONE_15,
                    colorEnum: EColor.BLUE,
                    storageEnum: EStorage.STORAGE_256,
                    productClassification: EProductClassification.IPHONE,
                    discountType: EDiscountType.PERCENTAGE,
                    colorHexCode: "#",
                    imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp",
                    discountValue: 10,
                    originalPrice: 1100,
                    stock: 5
                ),
                EventItem.Create(
                    eventItemId: EventItemId.Create(),
                    eventId: EventId.Of("611db6eb-3d64-474e-9e23-3517ad0df6ec"),
                    skuId: "690f4601e2295b9f94f23f63",
                    tenantId: "664355f845e56534956be32b",
                    branchId: "664357a235e84033bbd0e6b6",
                    iphoneModelEnum: EIphoneModel.IPHONE_15,
                    colorEnum: EColor.PINK,
                    storageEnum: EStorage.STORAGE_128,
                    productClassification: EProductClassification.IPHONE,
                    discountType: EDiscountType.PERCENTAGE,
                    colorHexCode: "#",
                    imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp",
                    discountValue: 8,
                    originalPrice: 1000,
                    stock: 5
                ),
                EventItem.Create(
                    eventItemId: EventItemId.Create(),
                    eventId: EventId.Of("611db6eb-3d64-474e-9e23-3517ad0df6ec"),
                    skuId: "690f4601e2295b9f94f23f69",
                    tenantId: "664355f845e56534956be32b",
                    branchId: "664357a235e84033bbd0e6b6",
                    iphoneModelEnum: EIphoneModel.IPHONE_15,
                    colorEnum: EColor.YELLOW,
                    storageEnum: EStorage.STORAGE_512,
                    productClassification: EProductClassification.IPHONE,
                    discountType: EDiscountType.PERCENTAGE,
                    colorHexCode: "#",
                    imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp",
                    discountValue: 9,
                    originalPrice: 1200,
                    stock: 5
                ),
                EventItem.Create(
                    eventItemId: EventItemId.Create(),
                    eventId: EventId.Of("611db6eb-3d64-474e-9e23-3517ad0df6ec"),
                    skuId: "690f4601e2295b9f94f23f79",
                    tenantId: "664355f845e56534956be32b",
                    branchId: "664357a235e84033bbd0e6b6",
                    iphoneModelEnum: EIphoneModel.IPHONE_15_PLUS,
                    colorEnum: EColor.PINK,
                    storageEnum: EStorage.STORAGE_512,
                    productClassification: EProductClassification.IPHONE,
                    discountType: EDiscountType.PERCENTAGE,
                    colorHexCode: "#",
                    imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp",
                    discountValue: 8,
                    originalPrice: 1200,
                    stock: 5
                ),
                EventItem.Create(
                    eventItemId: EventItemId.Create(),
                    eventId: EventId.Of("611db6eb-3d64-474e-9e23-3517ad0df6ec"),
                    skuId: "690f4601e2295b9f94f23f8c",
                    tenantId: "664355f845e56534956be32b",
                    branchId: "664357a235e84033bbd0e6b6",
                    iphoneModelEnum: EIphoneModel.IPHONE_16,
                    colorEnum: EColor.TEAL,
                    storageEnum: EStorage.STORAGE_256,
                    productClassification: EProductClassification.IPHONE,
                    discountType: EDiscountType.PERCENTAGE,
                    colorHexCode: "#",
                    imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811404/iphone-16-finish-select-202409-6-1inch-teal_gfumfa.webp",
                    discountValue: 12,
                    originalPrice: 1300,
                    stock: 5
                ),
                EventItem.Create(
                    eventItemId: EventItemId.Create(),
                    eventId: EventId.Of("611db6eb-3d64-474e-9e23-3517ad0df6ec"),
                    skuId: "690f4601e2295b9f94f23f99",
                    tenantId: "664355f845e56534956be32b",
                    branchId: "664357a235e84033bbd0e6b6",
                    iphoneModelEnum: EIphoneModel.IPHONE_16,
                    colorEnum: EColor.BLACK,
                    storageEnum: EStorage.STORAGE_512,
                    productClassification: EProductClassification.IPHONE,
                    discountType: EDiscountType.PERCENTAGE,
                    colorHexCode: "#",
                    imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811496/iphone-16-finish-select-202409-6-1inch-black_wnfzl5.webp",
                    discountValue: 11,
                    originalPrice: 1400,
                    stock: 5
                ),
                EventItem.Create(
                    eventItemId: EventItemId.Create(),
                    eventId: EventId.Of("611db6eb-3d64-474e-9e23-3517ad0df6ec"),
                    skuId: "690f4601e2295b9f94f23f8f",
                    tenantId: "664355f845e56534956be32b",
                    branchId: "664357a235e84033bbd0e6b6",
                    iphoneModelEnum: EIphoneModel.IPHONE_16,
                    colorEnum: EColor.PINK,
                    storageEnum: EStorage.STORAGE_128,
                    productClassification: EProductClassification.IPHONE,
                    discountType: EDiscountType.PERCENTAGE,
                    colorHexCode: "#",
                    imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811098/iphone-16-finish-select-202409-6-1inch-pink_q2saue.webp",
                    discountValue: 7,
                    originalPrice: 1200,
                    stock: 5
                ),
                EventItem.Create(
                    eventItemId: EventItemId.Create(),
                    eventId: EventId.Of("611db6eb-3d64-474e-9e23-3517ad0df6ec"),
                    skuId: "690f4601e2295b9f94f23f9d",
                    tenantId: "664355f845e56534956be32b",
                    branchId: "664357a235e84033bbd0e6b6",
                    iphoneModelEnum: EIphoneModel.IPHONE_16_PLUS,
                    colorEnum: EColor.ULTRAMARINE,
                    storageEnum: EStorage.STORAGE_512,
                    productClassification: EProductClassification.IPHONE,
                    discountType: EDiscountType.PERCENTAGE,
                    colorHexCode: "#",
                    imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp",
                    discountValue: 7,
                    originalPrice: 1400,
                    stock: 5
                ),
                EventItem.Create(
                    eventItemId: EventItemId.Create(),
                    eventId: EventId.Of("611db6eb-3d64-474e-9e23-3517ad0df6ec"),
                    skuId: "690f4601e2295b9f94f23fb0",
                    tenantId: "664355f845e56534956be32b",
                    branchId: "664357a235e84033bbd0e6b6",
                    iphoneModelEnum: EIphoneModel.IPHONE_16E,
                    colorEnum: EColor.WHITE,
                    storageEnum: EStorage.STORAGE_128,
                    productClassification: EProductClassification.IPHONE,
                    discountType: EDiscountType.PERCENTAGE,
                    colorHexCode: "#",
                    imageUrl: "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-16e-finish-select-202502-white_g1coja.webp",
                    discountValue: 6,
                    originalPrice: 1300,
                    stock: 5
                )
            };
        }
    }
}
