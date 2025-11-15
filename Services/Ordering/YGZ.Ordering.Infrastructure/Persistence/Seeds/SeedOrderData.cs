using YGZ.BuildingBlocks.Shared.Contracts.Products;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Infrastructure.Persistence.Seeds;

public static class SeedOrderData
{
    public static IEnumerable<UserId> UserIds => new List<UserId>
    {
        UserId.Of("7ecf88f8-3e55-40cc-92d0-5d3a5a5e228f"), // listShippingAddresses[0]
    };

    public static IEnumerable<ShippingAddress> ShippingAddresses => new List<ShippingAddress>
    {
        ShippingAddress.Create(contactName: "Bach Le",
                               contactEmail: "lov3rinve146@gmail.com",
                               contactPhoneNumber: "0909090909",
                               addressLine: "1060 Kha Van Can",
                               district: "Quận Thủ Đức",
                               province: "Thành phố Hồ Chí Minh",
                               country: "Vietnam"),
    };

    public static IEnumerable<string> TenantIds => new List<string>
            {
                "664355f845e56534956be32b", // Ware house
                "690e034dff79797b05b3bc89" // HCM TD KVC 1060
            };

    public static IEnumerable<string> SkuIdsInHCMTDKVC1060 => new List<string>
    {
        "691364b22451d4a9c6ca67db", // IPHONE-IPHONE_15-128GB-BLUE
    };

    public static IEnumerable<Order> Orders
    {
        get
        {
            var listUserIds = UserIds.ToList();
            var listShippingAddresses = ShippingAddresses.ToList();
            var listTenantId = TenantIds.ToList();
            var iPhone15ModelsList = iPhone15Models.ToList();
            var iPhone15ColorsList = iPhone15Colors.ToList();
            var iPhone16ModelsList = iPhone16Models.ToList();
            var iPhone16ColorsList = iPhone16Colors.ToList();
            var iPhone16EModelsList = iPhone16EModels.ToList();
            var iPhone16EColorsList = iPhone16EColors.ToList();
            var iPhone17ModelsList = iPhone17Models.ToList();
            var iPhone17ColorsList = iPhone17Colors.ToList();
            var iPhone17ProModelsList = iPhone17ProModels.ToList();
            var iPhone17ProColorsList = iPhone17ProColors.ToList();
            var iPhone17AirModelsList = iPhone17AirModels.ToList();
            var iPhone17AirColorsList = iPhone17AirColors.ToList();
            var StoragesList = Storages.ToList();
            var listSkuIdsInWarehouseList = SkuIdsInWarehouse.ToList();
            var modelSlugs = ModelSlugs.ToList();

            Dictionary<string, string> BuildDisplayImageUrls(List<string> modelNames,
                                                             List<string> storageOptions,
                                                             List<string> colorOptions,
                                                             Dictionary<string, string> colorImageUrls)
            {
                var dictionary = new Dictionary<string, string>();

                foreach (var model in modelNames)
                {
                    foreach (var storage in storageOptions)
                    {
                        foreach (var color in colorOptions)
                        {
                            if (colorImageUrls.TryGetValue(color, out var imageUrl))
                            {
                                var key = $"{model}-{storage}-{color}";
                                dictionary[key] = imageUrl;
                            }
                        }
                    }
                }

                return dictionary;
            }

            var iphone15displayImageUrls = new Dictionary<string, string>();

            var iphone15ColorImageUrls = new Dictionary<string, string>
            {
                { "BLUE",   "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp" },
                { "PINK",   "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-15-finish-select-202309-6-1inch-pink_j6v96t.webp" },
                { "YELLOW", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-15-finish-select-202309-6-1inch-yellow_pwviwe.webp" },
                { "GREEN",  "https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-15-finish-select-202309-6-1inch-green_yk0ln5.webp" },
                { "BLACK",  "https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-15-finish-select-202309-6-1inch-black_hhhvfs.webp" }
            };

            var iphone16ColorImageUrls = new Dictionary<string, string>
            {
                { "ULTRAMARINE", "https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp" },
                { "TEAL", "https://res.cloudinary.com/delkyrtji/image/upload/v1744811404/iphone-16-finish-select-202409-6-1inch-teal_gfumfa.webp" },
                { "PINK", "https://res.cloudinary.com/delkyrtji/image/upload/v1744811098/iphone-16-finish-select-202409-6-1inch-pink_q2saue.webp" },
                { "WHITE", "https://res.cloudinary.com/delkyrtji/image/upload/v1744811448/iphone-16-finish-select-202409-6-1inch-white_ghumel.webp" },
                { "BLACK", "https://res.cloudinary.com/delkyrtji/image/upload/v1744811496/iphone-16-finish-select-202409-6-1inch-black_wnfzl5.webp" }
            };

            var iphone16EColorImageUrls = new Dictionary<string, string>
            {
                { "WHITE", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-16e-finish-select-202502-white_g1coja.webp" },
                { "BLACK", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-16e-finish-select-202502-black_yq48ki.webp" }
            };

            var iphone17ColorImageUrls = new Dictionary<string, string>
            {
                { "LAVENDER", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-17-finish-select-202509-lavender_ttymfa.webp" },
                { "SAGE", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-17-finish-select-202509-sage_aw371h.webp" },
                { "MIST_BLUE", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-17-finish-select-202509-mistblue_gcqb5o.webp" },
                { "WHITE", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-17-finish-select-202509-white_hphgpt.webp" },
                { "BLACK", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960469/iphone-17-finish-select-202509-black_df2lsp.webp" }
            };


            var iphone17ProColorImageUrls = new Dictionary<string, string>
            {
                { "SILVER", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-17-pro-finish-select-202509-6-3inch-silver_p9eegd.webp" },
                { "COSMIC_ORANGE", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-17-pro-finish-select-202509-6-3inch-cosmicorange_ye9ms2.webp" },
                { "DEEP_BLUE", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-17-pro-finish-select-202509-6-3inch-deepblue_xhdpyx.webp" }
            };

            var iphone17AirColorImageUrls = new Dictionary<string, string>
            {
                { "SKY_BLUE", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-air-finish-select-202509-skyblue_hhd2cg.webp" },
                { "LIGHT_GOLD", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-air-finish-select-202509-lightgold_zegr3u.webp" },
                { "CLOUD_WHITE", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-air-finish-select-202509-cloudwhite_ubwvpe.webp" },
                { "SPACE_BLACK", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960447/iphone-air-finish-select-202509-spaceblack_xtp6d4.webp" }
            };

            var iphone15ModelNames = new List<string> { "IPHONE_15", "IPHONE_15_PLUS" };
            var iphone15Storages = StoragesList.Take(4).ToList(); // first four storages
            var iphone15Colors = iPhone15ColorsList.Take(5).ToList(); // first five colors
            iphone15displayImageUrls = BuildDisplayImageUrls(iphone15ModelNames, iphone15Storages, iphone15Colors, iphone15ColorImageUrls);

            var iphone16displayImageUrls = BuildDisplayImageUrls(iPhone16ModelsList, StoragesList, iPhone16ColorsList, iphone16ColorImageUrls);
            var iphone16EDisplayImageUrls = BuildDisplayImageUrls(iPhone16EModelsList, StoragesList, iPhone16EColorsList, iphone16EColorImageUrls);
            var iphone17displayImageUrls = BuildDisplayImageUrls(iPhone17ModelsList, StoragesList, iPhone17ColorsList, iphone17ColorImageUrls);
            var iphone17ProDisplayImageUrls = BuildDisplayImageUrls(iPhone17ProModelsList, StoragesList, iPhone17ProColorsList, iphone17ProColorImageUrls);
            var iphone17AirDisplayImageUrls = BuildDisplayImageUrls(iPhone17AirModelsList, StoragesList, iPhone17AirColorsList, iphone17AirColorImageUrls);

            string GetDisplayImageUrl(string model, string color, string storage)
            {
                var key = $"{model}-{storage}-{color}";

                if (model.StartsWith("IPHONE_15") && iphone15displayImageUrls.TryGetValue(key, out var url15))
                {
                    return url15;
                }

                if (model.StartsWith("IPHONE_16") && !model.Contains("E") && iphone16displayImageUrls.TryGetValue(key, out var url16))
                {
                    return url16;
                }

                if (model.StartsWith("IPHONE_16E") && iphone16EDisplayImageUrls.TryGetValue(key, out var url16E))
                {
                    return url16E;
                }

                if (model.StartsWith("IPHONE_17") && !model.Contains("PRO") && !model.Contains("AIR") && iphone17displayImageUrls.TryGetValue(key, out var url17))
                {
                    return url17;
                }

                if (model.StartsWith("IPHONE_17_PRO") && iphone17ProDisplayImageUrls.TryGetValue(key, out var url17Pro))
                {
                    return url17Pro;
                }

                if (model.StartsWith("IPHONE_17_AIR") && iphone17AirDisplayImageUrls.TryGetValue(key, out var url17Air))
                {
                    return url17Air;
                }

                return string.Empty;
            }



            // 40 variant items of iPhone 15
            List<OrderItem> orderItemsList1 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-BLUE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: listSkuIdsInWarehouseList[0],
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[0],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[0], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[0], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["02_01_2025"]),
            };

            List<OrderItem> orderItemsList2 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-BLUE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: listSkuIdsInWarehouseList[0],
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[0],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[0], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[0], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["03_01_2025"]),
            };

            List<OrderItem> orderItemsList3 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-BLUE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: listSkuIdsInWarehouseList[0],
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[0],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[0], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[0], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["04_01_2025"]),
            };

            List<OrderItem> orderItemsList4 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-BLUE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: listSkuIdsInWarehouseList[0],
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[0],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[0], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[0], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["05_01_2025"]),
            };

            List<OrderItem> orderItemsList5 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-PINK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: listSkuIdsInWarehouseList[0],
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[1],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[1], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[1], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["06_01_2025"]),
            };

            List<OrderItem> orderItemsList6 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-PINK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: listSkuIdsInWarehouseList[0],
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[1],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[1], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[1], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["07_01_2025"]),
            };

            List<OrderItem> orderItemsList7 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-PINK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: listSkuIdsInWarehouseList[0],
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[1],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[1], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[1], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["08_01_2025"]),
            };

            List<OrderItem> orderItemsList8 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-PINK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: listSkuIdsInWarehouseList[0],
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[1],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[1], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[1], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["09_01_2025"]),
            };

            List<OrderItem> orderItemsList9 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-YELLOW-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: listSkuIdsInWarehouseList[0],
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[2],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[2], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[2], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["10_01_2025"]),
            };

            List<OrderItem> orderItemsList10 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-YELLOW-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: listSkuIdsInWarehouseList[0],
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[2],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[2], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[2], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["01_01_2025"]),
            };

            List<OrderItem> orderItemsList11 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-YELLOW-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[2],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[2], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[2], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["11_01_2025"]),
            };

            List<OrderItem> orderItemsList12 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-YELLOW-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[2],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[2], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[2], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["12_01_2025"]),
            };

            List<OrderItem> orderItemsList13 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-GREEN-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[3],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[3], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[3], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["13_01_2025"]),
            };

            List<OrderItem> orderItemsList14 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-GREEN-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[3],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[3], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[3], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["14_01_2025"]),
            };

            List<OrderItem> orderItemsList15 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-GREEN-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[3],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[3], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[3], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["15_01_2025"]),
            };

            List<OrderItem> orderItemsList16 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-GREEN-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[3],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[3], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[3], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["16_01_2025"]),
            };

            List<OrderItem> orderItemsList17 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-BLACK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[4],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[4], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[4], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["17_01_2025"]),
            };

            List<OrderItem> orderItemsList18 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-BLACK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[4],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[4], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[4], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["18_01_2025"]),
            };

            List<OrderItem> orderItemsList19 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-BLACK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[4],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[4], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[4], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["19_01_2025"]),
            };

            List<OrderItem> orderItemsList20 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-BLACK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[0],
                                 colorName: iPhone15ColorsList[4],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[0], iPhone15ColorsList[4], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[0], iPhone15ColorsList[4], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["20_01_2025"]),
            };

            List<OrderItem> orderItemsList21 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLUE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[0],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[0], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[0], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["21_01_2025"]),
            };

            List<OrderItem> orderItemsList22 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLUE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[0],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[0], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[0], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["22_01_2025"]),
            };

            List<OrderItem> orderItemsList23 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLUE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[0],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[0], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[0], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["23_01_2025"]),
            };

            List<OrderItem> orderItemsList24 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLUE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[0],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[0], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[0], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["24_01_2025"]),
            };

            List<OrderItem> orderItemsList25 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-PINK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[1],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[1], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[1], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["25_01_2025"]),
            };

            List<OrderItem> orderItemsList26 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-PINK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[1],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[1], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[1], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["26_01_2025"]),
            };

            List<OrderItem> orderItemsList27 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-PINK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[1],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[1], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[1], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["27_01_2025"]),
            };

            List<OrderItem> orderItemsList28 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-PINK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[1],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[1], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[1], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["28_01_2025"]),
            };

            List<OrderItem> orderItemsList29 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-YELLOW-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[2],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[2], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[2], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["29_01_2025"]),
            };

            List<OrderItem> orderItemsList30 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-YELLOW-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[2],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[2], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[2], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["30_01_2025"]),
            };

            List<OrderItem> orderItemsList31 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-YELLOW-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[2],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[2], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[2], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["31_01_2025"]),
            };

            List<OrderItem> orderItemsList32 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-YELLOW-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[2],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[2], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[2], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["01_02_2025"]),
            };

            List<OrderItem> orderItemsList33 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-GREEN-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[3],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[3], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[3], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["02_02_2025"]),
            };

            List<OrderItem> orderItemsList34 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-GREEN-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[3],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[3], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[3], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["03_02_2025"]),
            };

            List<OrderItem> orderItemsList35 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-GREEN-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[3],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[3], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[3], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["04_02_2025"]),
            };

            List<OrderItem> orderItemsList36 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-GREEN-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[3],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[3], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[3], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["05_02_2025"]),
            };

            List<OrderItem> orderItemsList37 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLACK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[4],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[4], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[4], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["06_02_2025"]),
            };

            List<OrderItem> orderItemsList38 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLACK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[4],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[4], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[4], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["07_02_2025"]),
            };

            List<OrderItem> orderItemsList39 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLACK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[4],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[4], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[4], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["08_02_2025"]),
            };

            List<OrderItem> orderItemsList40 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLACK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_15"],
                                 modelName: iPhone15ModelsList[1],
                                 colorName: iPhone15ColorsList[4],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone15ModelsList[1], iPhone15ColorsList[4], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone15ModelsList[1], iPhone15ColorsList[4], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone15ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["09_02_2025"]),
            };


            // 40 variant items of iPhone 16
            List<OrderItem> orderItemsList41 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-ULTRAMARINE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[0],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[0], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[0], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["10_02_2025"]),
            };

            List<OrderItem> orderItemsList42 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-ULTRAMARINE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[0],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[0], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[0], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["11_02_2025"]),
            };

            List<OrderItem> orderItemsList43 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-ULTRAMARINE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[0],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[0], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[0], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["12_02_2025"]),
            };

            List<OrderItem> orderItemsList44 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-ULTRAMARINE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[0],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[0], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[0], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["13_02_2025"]),
            };

            List<OrderItem> orderItemsList45 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-TEAL-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[1],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[1], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[1], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["14_02_2025"]),
            };

            List<OrderItem> orderItemsList46 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-TEAL-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[1],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[1], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[1], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["15_02_2025"]),
            };

            List<OrderItem> orderItemsList47 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-TEAL-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[1],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[1], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[1], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["16_02_2025"]),
            };

            List<OrderItem> orderItemsList48 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-TEAL-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[1],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[1], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[1], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["17_02_2025"]),
            };

            List<OrderItem> orderItemsList49 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-PINK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[2],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[2], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[2], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["18_02_2025"]),
            };

            List<OrderItem> orderItemsList50 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-PINK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[2],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[2], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[2], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["19_02_2025"]),
            };

            List<OrderItem> orderItemsList51 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-PINK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[2],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[2], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[2], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["20_02_2025"]),
            };

            List<OrderItem> orderItemsList52 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-PINK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[2],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[2], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[2], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["21_02_2025"]),
            };

            List<OrderItem> orderItemsList53 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-WHITE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[3],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[3], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[3], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["22_02_2025"]),
            };

            List<OrderItem> orderItemsList54 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-WHITE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[3],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[3], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[3], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["23_02_2025"]),
            };

            List<OrderItem> orderItemsList55 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-WHITE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[3],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[3], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[3], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["24_02_2025"]),
            };

            List<OrderItem> orderItemsList56 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-WHITE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[3],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[3], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[3], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["25_02_2025"]),
            };

            List<OrderItem> orderItemsList57 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-BLACK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[4],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[4], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[4], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["26_02_2025"]),
            };

            List<OrderItem> orderItemsList58 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-BLACK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[4],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[4], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[4], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["27_02_2025"]),
            };

            List<OrderItem> orderItemsList59 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-BLACK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[4],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[4], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[4], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["28_02_2025"]),
            };

            List<OrderItem> orderItemsList60 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-BLACK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[0],
                                 colorName: iPhone16ColorsList[4],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[0], iPhone16ColorsList[4], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[0], iPhone16ColorsList[4], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["01_03_2025"]),
            };

            List<OrderItem> orderItemsList61 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-ULTRAMARINE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[0],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[0], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[0], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["02_03_2025"]),
            };

            List<OrderItem> orderItemsList62 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-ULTRAMARINE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[0],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[0], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[0], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["03_03_2025"]),
            };

            List<OrderItem> orderItemsList63 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-ULTRAMARINE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[0],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[0], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[0], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["04_03_2025"]),
            };

            List<OrderItem> orderItemsList64 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-ULTRAMARINE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[0],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[0], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[0], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["05_03_2025"]),
            };

            List<OrderItem> orderItemsList65 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-TEAL-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[1],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[1], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[1], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["06_03_2025"]),
            };

            List<OrderItem> orderItemsList66 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-TEAL-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[1],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[1], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[1], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["07_03_2025"]),
            };

            List<OrderItem> orderItemsList67 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-TEAL-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[1],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[1], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[1], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["08_03_2025"]),
            };

            List<OrderItem> orderItemsList68 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-TEAL-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[1],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[1], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[1], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["09_03_2025"]),
            };

            List<OrderItem> orderItemsList69 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-PINK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[2],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[2], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[2], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["10_03_2025"]),
            };

            List<OrderItem> orderItemsList70 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-PINK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[2],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[2], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[2], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["11_03_2025"]),
            };

            List<OrderItem> orderItemsList71 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-PINK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[2],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[2], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[2], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["12_03_2025"]),
            };

            List<OrderItem> orderItemsList72 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-PINK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[2],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[2], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[2], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["13_03_2025"]),
            };

            List<OrderItem> orderItemsList73 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-WHITE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[3],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[3], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[3], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["14_03_2025"]),
            };

            List<OrderItem> orderItemsList74 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-WHITE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[3],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[3], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[3], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["15_03_2025"]),
            };

            List<OrderItem> orderItemsList75 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-WHITE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[3],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[3], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[3], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["16_03_2025"]),
            };

            List<OrderItem> orderItemsList76 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-WHITE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[3],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[3], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[3], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["17_03_2025"]),
            };

            List<OrderItem> orderItemsList77 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-BLACK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[4],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[4], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[4], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["18_03_2025"]),
            };

            List<OrderItem> orderItemsList78 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-BLACK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[4],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[4], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[4], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["19_03_2025"]),
            };

            List<OrderItem> orderItemsList79 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-BLACK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[4],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[4], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[4], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["20_03_2025"]),
            };

            List<OrderItem> orderItemsList80 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-BLACK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16"],
                                 modelName: iPhone16ModelsList[1],
                                 colorName: iPhone16ColorsList[4],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16ModelsList[1], iPhone16ColorsList[4], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16ModelsList[1], iPhone16ColorsList[4], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone16ModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["21_03_2025"]),
            };


            // 8 variant items of iPhone 16e
            List<OrderItem> orderItemsList81 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-WHITE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16E"],
                                 modelName: iPhone16EModelsList[0],
                                 colorName: iPhone16EColorsList[0],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16EModelsList[0], iPhone16EColorsList[0], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16EModelsList[0], iPhone16EColorsList[0], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone16EModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["22_03_2025"]),
            };

            List<OrderItem> orderItemsList82 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-WHITE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16E"],
                                 modelName: iPhone16EModelsList[0],
                                 colorName: iPhone16EColorsList[0],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16EModelsList[0], iPhone16EColorsList[0], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16EModelsList[0], iPhone16EColorsList[0], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone16EModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["23_03_2025"]),
            };

            List<OrderItem> orderItemsList83 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-WHITE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16E"],
                                 modelName: iPhone16EModelsList[0],
                                 colorName: iPhone16EColorsList[0],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16EModelsList[0], iPhone16EColorsList[0], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16EModelsList[0], iPhone16EColorsList[0], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone16EModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["24_03_2025"]),
            };

            List<OrderItem> orderItemsList84 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-WHITE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16E"],
                                 modelName: iPhone16EModelsList[0],
                                 colorName: iPhone16EColorsList[0],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16EModelsList[0], iPhone16EColorsList[0], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16EModelsList[0], iPhone16EColorsList[0], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone16EModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["25_03_2025"]),
            };

            List<OrderItem> orderItemsList85 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-BLACK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16E"],
                                 modelName: iPhone16EModelsList[0],
                                 colorName: iPhone16EColorsList[1],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16EModelsList[0], iPhone16EColorsList[1], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16EModelsList[0], iPhone16EColorsList[1], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone16EModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["26_03_2025"]),
            };

            List<OrderItem> orderItemsList86 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-BLACK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16E"],
                                 modelName: iPhone16EModelsList[0],
                                 colorName: iPhone16EColorsList[1],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16EModelsList[0], iPhone16EColorsList[1], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16EModelsList[0], iPhone16EColorsList[1], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone16EModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["27_03_2025"]),
            };

            List<OrderItem> orderItemsList87 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-BLACK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16E"],
                                 modelName: iPhone16EModelsList[0],
                                 colorName: iPhone16EColorsList[1],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16EModelsList[0], iPhone16EColorsList[1], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16EModelsList[0], iPhone16EColorsList[1], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone16EModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["28_03_2025"]),
            };

            List<OrderItem> orderItemsList88 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-BLACK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_16E"],
                                 modelName: iPhone16EModelsList[0],
                                 colorName: iPhone16EColorsList[1],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone16EModelsList[0], iPhone16EColorsList[1], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone16EModelsList[0], iPhone16EColorsList[1], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone16EModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["29_03_2025"]),
            };


            // 20 variant items of iPhone 17
            List<OrderItem> orderItemsList89 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-LAVENDER-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[0],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[0], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[0], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["30_03_2025"]),
            };

            List<OrderItem> orderItemsList90 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-LAVENDER-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[0],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[0], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[0], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["31_03_2025"]),
            };

            List<OrderItem> orderItemsList91 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-LAVENDER-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[0],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[0], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[0], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["01_04_2025"]),
            };

            List<OrderItem> orderItemsList92 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-LAVENDER-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[0],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[0], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[0], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["02_04_2025"]),
            };

            List<OrderItem> orderItemsList93 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-SAGE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[1],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[1], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[1], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["03_04_2025"]),
            };

            List<OrderItem> orderItemsList94 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-SAGE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[1],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[1], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[1], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["04_04_2025"]),
            };

            List<OrderItem> orderItemsList95 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-SAGE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[1],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[1], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[1], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["05_04_2025"]),
            };

            List<OrderItem> orderItemsList96 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-SAGE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[1],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[1], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[1], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["06_04_2025"]),
            };

            List<OrderItem> orderItemsList97 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-MIST_BLUE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[2],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[2], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[2], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["07_04_2025"]),
            };

            List<OrderItem> orderItemsList98 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-MIST_BLUE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[2],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[2], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[2], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["08_04_2025"]),
            };

            List<OrderItem> orderItemsList99 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-MIST_BLUE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[2],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[2], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[2], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["09_04_2025"]),
            };

            List<OrderItem> orderItemsList100 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-MIST_BLUE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[2],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[2], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[2], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["10_04_2025"]),
            };

            List<OrderItem> orderItemsList101 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-WHITE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[3],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[3], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[3], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["11_04_2025"]),
            };

            List<OrderItem> orderItemsList102 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-WHITE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[3],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[3], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[3], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["12_04_2025"]),
            };

            List<OrderItem> orderItemsList103 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-WHITE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[3],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[3], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[3], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["13_04_2025"]),
            };

            List<OrderItem> orderItemsList104 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-WHITE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[3],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[3], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[3], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["14_04_2025"]),
            };

            List<OrderItem> orderItemsList105 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-BLACK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[4],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[4], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[4], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["15_04_2025"]),
            };

            List<OrderItem> orderItemsList106 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-BLACK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[4],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[4], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[4], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["16_04_2025"]),
            };

            List<OrderItem> orderItemsList107 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-BLACK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[4],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[4], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[4], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["17_04_2025"]),
            };

            List<OrderItem> orderItemsList108 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-BLACK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17"],
                                 modelName: iPhone17ModelsList[0],
                                 colorName: iPhone17ColorsList[4],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ModelsList[0], iPhone17ColorsList[4], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ModelsList[0], iPhone17ColorsList[4], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone17ModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["18_04_2025"]),
            };



            // 24 variant items of iPhone 17 Pro
            List<OrderItem> orderItemsList109 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-SILVER-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[0],
                                 colorName: iPhone17ProColorsList[0],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[0], iPhone17ProColorsList[0], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[0], iPhone17ProColorsList[0], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["19_04_2025"]),
            };

            List<OrderItem> orderItemsList110 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-SILVER-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[0],
                                 colorName: iPhone17ProColorsList[0],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[0], iPhone17ProColorsList[0], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[0], iPhone17ProColorsList[0], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["20_04_2025"]),
            };

            List<OrderItem> orderItemsList111 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-SILVER-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[0],
                                 colorName: iPhone17ProColorsList[0],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[0], iPhone17ProColorsList[0], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[0], iPhone17ProColorsList[0], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["21_04_2025"]),
            };

            List<OrderItem> orderItemsList112 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-SILVER-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[0],
                                 colorName: iPhone17ProColorsList[0],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[0], iPhone17ProColorsList[0], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[0], iPhone17ProColorsList[0], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["22_04_2025"]),
            };

            List<OrderItem> orderItemsList113 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-COSMIC_ORANGE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[0],
                                 colorName: iPhone17ProColorsList[1],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[0], iPhone17ProColorsList[1], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[0], iPhone17ProColorsList[1], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["23_04_2025"]),
            };

            List<OrderItem> orderItemsList114 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-COSMIC_ORANGE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[0],
                                 colorName: iPhone17ProColorsList[1],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[0], iPhone17ProColorsList[1], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[0], iPhone17ProColorsList[1], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["24_04_2025"]),
            };

            List<OrderItem> orderItemsList115 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-COSMIC_ORANGE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[0],
                                 colorName: iPhone17ProColorsList[1],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[0], iPhone17ProColorsList[1], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[0], iPhone17ProColorsList[1], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["25_04_2025"]),
            };

            List<OrderItem> orderItemsList116 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-COSMIC_ORANGE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[0],
                                 colorName: iPhone17ProColorsList[1],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[0], iPhone17ProColorsList[1], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[0], iPhone17ProColorsList[1], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["26_04_2025"]),
            };

            List<OrderItem> orderItemsList117 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-DEEP_BLUE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[0],
                                 colorName: iPhone17ProColorsList[2],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[0], iPhone17ProColorsList[2], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[0], iPhone17ProColorsList[2], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["27_04_2025"]),
            };

            List<OrderItem> orderItemsList118 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-DEEP_BLUE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[0],
                                 colorName: iPhone17ProColorsList[2],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[0], iPhone17ProColorsList[2], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[0], iPhone17ProColorsList[2], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["28_04_2025"]),
            };

            List<OrderItem> orderItemsList119 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-DEEP_BLUE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[0],
                                 colorName: iPhone17ProColorsList[2],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[0], iPhone17ProColorsList[2], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[0], iPhone17ProColorsList[2], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["29_04_2025"]),
            };

            List<OrderItem> orderItemsList120 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-DEEP_BLUE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[0],
                                 colorName: iPhone17ProColorsList[2],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[0], iPhone17ProColorsList[2], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[0], iPhone17ProColorsList[2], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["30_04_2025"]),
            };

            List<OrderItem> orderItemsList121 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-SILVER-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[1],
                                 colorName: iPhone17ProColorsList[0],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[1], iPhone17ProColorsList[0], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[1], iPhone17ProColorsList[0], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["01_05_2025"]),
            };

            List<OrderItem> orderItemsList122 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-SILVER-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[1],
                                 colorName: iPhone17ProColorsList[0],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[1], iPhone17ProColorsList[0], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[1], iPhone17ProColorsList[0], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["02_05_2025"]),
            };

            List<OrderItem> orderItemsList123 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-SILVER-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[1],
                                 colorName: iPhone17ProColorsList[0],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[1], iPhone17ProColorsList[0], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[1], iPhone17ProColorsList[0], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["03_05_2025"]),
            };

            List<OrderItem> orderItemsList124 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-SILVER-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[1],
                                 colorName: iPhone17ProColorsList[0],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[1], iPhone17ProColorsList[0], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[1], iPhone17ProColorsList[0], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["04_05_2025"]),
            };

            List<OrderItem> orderItemsList125 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-COSMIC_ORANGE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[1],
                                 colorName: iPhone17ProColorsList[1],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[1], iPhone17ProColorsList[1], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[1], iPhone17ProColorsList[1], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["05_05_2025"]),
            };

            List<OrderItem> orderItemsList126 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-COSMIC_ORANGE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[1],
                                 colorName: iPhone17ProColorsList[1],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[1], iPhone17ProColorsList[1], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[1], iPhone17ProColorsList[1], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["06_05_2025"]),
            };

            List<OrderItem> orderItemsList127 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-COSMIC_ORANGE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[1],
                                 colorName: iPhone17ProColorsList[1],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[1], iPhone17ProColorsList[1], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[1], iPhone17ProColorsList[1], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["07_05_2025"]),
            };

            List<OrderItem> orderItemsList128 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-COSMIC_ORANGE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[1],
                                 colorName: iPhone17ProColorsList[1],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[1], iPhone17ProColorsList[1], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[1], iPhone17ProColorsList[1], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["08_05_2025"]),
            };

            List<OrderItem> orderItemsList129 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-DEEP_BLUE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[1],
                                 colorName: iPhone17ProColorsList[2],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[1], iPhone17ProColorsList[2], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[1], iPhone17ProColorsList[2], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["09_05_2025"]),
            };

            List<OrderItem> orderItemsList130 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-DEEP_BLUE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[1],
                                 colorName: iPhone17ProColorsList[2],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[1], iPhone17ProColorsList[2], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[1], iPhone17ProColorsList[2], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["10_05_2025"]),
            };

            List<OrderItem> orderItemsList131 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-DEEP_BLUE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[1],
                                 colorName: iPhone17ProColorsList[2],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[1], iPhone17ProColorsList[2], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[1], iPhone17ProColorsList[2], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["11_05_2025"]),
            };

            List<OrderItem> orderItemsList132 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-DEEP_BLUE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_PRO"],
                                 modelName: iPhone17ProModelsList[1],
                                 colorName: iPhone17ProColorsList[2],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17ProModelsList[1], iPhone17ProColorsList[2], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17ProModelsList[1], iPhone17ProColorsList[2], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone17ProModelsList[1]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["12_05_2025"]),
            };


            // 16 variant items of iPhone 17 Air
            List<OrderItem> orderItemsList133 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SKY_BLUE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[0],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[0], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[0], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["13_05_2025"]),
            };

            List<OrderItem> orderItemsList134 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SKY_BLUE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[0],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[0], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[0], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["14_05_2025"]),
            };

            List<OrderItem> orderItemsList135 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SKY_BLUE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[0],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[0], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[0], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["15_05_2025"]),
            };

            List<OrderItem> orderItemsList136 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SKY_BLUE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[0],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[0], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[0], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["16_05_2025"]),
            };

            List<OrderItem> orderItemsList137 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-LIGHT_GOLD-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[1],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[1], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[1], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["17_05_2025"]),
            };

            List<OrderItem> orderItemsList138 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-LIGHT_GOLD-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[1],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[1], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[1], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["18_05_2025"]),
            };

            List<OrderItem> orderItemsList139 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-LIGHT_GOLD-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[1],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[1], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[1], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["19_05_2025"]),
            };

            List<OrderItem> orderItemsList140 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-LIGHT_GOLD-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[1],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[1], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[1], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["20_05_2025"]),
            };

            List<OrderItem> orderItemsList141 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-CLOUD_WHITE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[2],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[2], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[2], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["21_05_2025"]),
            };

            List<OrderItem> orderItemsList142 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-CLOUD_WHITE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[2],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[2], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[2], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["22_05_2025"]),
            };

            List<OrderItem> orderItemsList143 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-CLOUD_WHITE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[2],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[2], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[2], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["23_05_2025"]),
            };

            List<OrderItem> orderItemsList144 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-CLOUD_WHITE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[2],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[2], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[2], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["24_05_2025"]),
            };

            List<OrderItem> orderItemsList145 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SPACE_BLACK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[3],
                                 storageName: StoragesList[0],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[3], StoragesList[0]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[3], StoragesList[0]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["25_05_2025"]),
            };

            List<OrderItem> orderItemsList146 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SPACE_BLACK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[3],
                                 storageName: StoragesList[1],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[3], StoragesList[1]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[3], StoragesList[1]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["26_05_2025"]),
            };

            List<OrderItem> orderItemsList147 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SPACE_BLACK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[3],
                                 storageName: StoragesList[2],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[3], StoragesList[2]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[3], StoragesList[2]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["27_05_2025"]),
            };

            List<OrderItem> orderItemsList148 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SPACE_BLACK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: null,
                                 orderId: OrderId.Create(),
                                 skuId: Guid.NewGuid().ToString("N"),
                                 modelId: ModelIds["IPHONE_17_AIR"],
                                 modelName: iPhone17AirModelsList[0],
                                 colorName: iPhone17AirColorsList[3],
                                 storageName: StoragesList[3],
                                 unitPrice: GetPriceFromSkuPriceList(iPhone17AirModelsList[0], iPhone17AirColorsList[3], StoragesList[3]),
                                 displayImageUrl: GetDisplayImageUrl(iPhone17AirModelsList[0], iPhone17AirColorsList[3], StoragesList[3]),
                                 modelSlug: GetModelSlug(iPhone17AirModelsList[0]),
                                 quantity: 1,
                                 promotionId: null,
                                 promotionType: null,
                                 discountType: null,
                                 discountValue: null,
                                 discountAmount: null,
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["28_05_2025"]),
            };


















            // 40 orders of iPhone 15
            var order1 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: null,
                                      customerId: listUserIds[0],
                                      customerPublicKey: null,
                                      tx: null,
                                      code: Code.GenerateCode(),
                                      paymentMethod: EPaymentMethod.COD,
                                      orderStatus: EOrderStatus.DELIVERED,
                                      shippingAddress: listShippingAddresses[0],
                                      promotionId: null,
                                      promotionType: null,
                                      discountType: null,
                                      discountValue: null,
                                      discountAmount: null,
                                      createdAt: CreatedAtDateTimes["01_01_2025"]);
            order1.AddOrderItem(orderItemsList1[0]);
            order1.TotalAmount = orderItemsList1.Sum(x => x.SubTotalAmount);

            var order2 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: null,
                                      customerId: listUserIds[0],
                                      customerPublicKey: null,
                                      tx: null,
                                      code: Code.GenerateCode(),
                                      paymentMethod: EPaymentMethod.COD,
                                      orderStatus: EOrderStatus.DELIVERED,
                                      shippingAddress: listShippingAddresses[0],
                                      promotionId: null,
                                      promotionType: null,
                                      discountType: null,
                                      discountValue: null,
                                      discountAmount: null,
                                      createdAt: CreatedAtDateTimes["02_01_2025"]);
            order2.AddOrderItem(orderItemsList2[0]);
            order2.TotalAmount = orderItemsList2.Sum(x => x.SubTotalAmount);

            var order3 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: null,
                                      customerId: listUserIds[0],
                                      customerPublicKey: null,
                                      tx: null,
                                      code: Code.GenerateCode(),
                                      paymentMethod: EPaymentMethod.COD,
                                      orderStatus: EOrderStatus.DELIVERED,
                                      shippingAddress: listShippingAddresses[0],
                                      promotionId: null,
                                      promotionType: null,
                                      discountType: null,
                                      discountValue: null,
                                      discountAmount: null,
                                      createdAt: CreatedAtDateTimes["03_01_2025"]);
            order3.AddOrderItem(orderItemsList3[0]);
            order3.TotalAmount = orderItemsList3.Sum(x => x.SubTotalAmount);

            var order4 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: null,
                                      customerId: listUserIds[0],
                                      customerPublicKey: null,
                                      tx: null,
                                      code: Code.GenerateCode(),
                                      paymentMethod: EPaymentMethod.COD,
                                      orderStatus: EOrderStatus.DELIVERED,
                                      shippingAddress: listShippingAddresses[0],
                                      promotionId: null,
                                      promotionType: null,
                                      discountType: null,
                                      discountValue: null,
                                      discountAmount: null,
                                      createdAt: CreatedAtDateTimes["04_01_2025"]);
            order4.AddOrderItem(orderItemsList4[0]);
            order4.TotalAmount = orderItemsList4.Sum(x => x.SubTotalAmount);

            var order5 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: null,
                                      customerId: listUserIds[0],
                                      customerPublicKey: null,
                                      tx: null,
                                      code: Code.GenerateCode(),
                                      paymentMethod: EPaymentMethod.COD,
                                      orderStatus: EOrderStatus.DELIVERED,
                                      shippingAddress: listShippingAddresses[0],
                                      promotionId: null,
                                      promotionType: null,
                                      discountType: null,
                                      discountValue: null,
                                      discountAmount: null,
                                      createdAt: CreatedAtDateTimes["05_01_2025"]);
            order5.AddOrderItem(orderItemsList5[0]);
            order5.TotalAmount = orderItemsList5.Sum(x => x.SubTotalAmount);

            var order6 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: null,
                                      customerId: listUserIds[0],
                                      customerPublicKey: null,
                                      tx: null,
                                      code: Code.GenerateCode(),
                                      paymentMethod: EPaymentMethod.COD,
                                      orderStatus: EOrderStatus.DELIVERED,
                                      shippingAddress: listShippingAddresses[0],
                                      promotionId: null,
                                      promotionType: null,
                                      discountType: null,
                                      discountValue: null,
                                      discountAmount: null,
                                      createdAt: CreatedAtDateTimes["06_01_2025"]);
            order6.AddOrderItem(orderItemsList6[0]);
            order6.TotalAmount = orderItemsList6.Sum(x => x.SubTotalAmount);

            var order7 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: null,
                                      customerId: listUserIds[0],
                                      customerPublicKey: null,
                                      tx: null,
                                      code: Code.GenerateCode(),
                                      paymentMethod: EPaymentMethod.COD,
                                      orderStatus: EOrderStatus.DELIVERED,
                                      shippingAddress: listShippingAddresses[0],
                                      promotionId: null,
                                      promotionType: null,
                                      discountType: null,
                                      discountValue: null,
                                      discountAmount: null,
                                      createdAt: CreatedAtDateTimes["07_01_2025"]);
            order7.AddOrderItem(orderItemsList7[0]);
            order7.TotalAmount = orderItemsList7.Sum(x => x.SubTotalAmount);

            var order8 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: null,
                                      customerId: listUserIds[0],
                                      customerPublicKey: null,
                                      tx: null,
                                      code: Code.GenerateCode(),
                                      paymentMethod: EPaymentMethod.COD,
                                      orderStatus: EOrderStatus.DELIVERED,
                                      shippingAddress: listShippingAddresses[0],
                                      promotionId: null,
                                      promotionType: null,
                                      discountType: null,
                                      discountValue: null,
                                      discountAmount: null,
                                      createdAt: CreatedAtDateTimes["08_01_2025"]);
            order8.AddOrderItem(orderItemsList8[0]);
            order8.TotalAmount = orderItemsList8.Sum(x => x.SubTotalAmount);

            var order9 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: null,
                                      customerId: listUserIds[0],
                                      customerPublicKey: null,
                                      tx: null,
                                      code: Code.GenerateCode(),
                                      paymentMethod: EPaymentMethod.COD,
                                      orderStatus: EOrderStatus.DELIVERED,
                                      shippingAddress: listShippingAddresses[0],
                                      promotionId: null,
                                      promotionType: null,
                                      discountType: null,
                                      discountValue: null,
                                      discountAmount: null,
                                      createdAt: CreatedAtDateTimes["09_01_2025"]);
            order9.AddOrderItem(orderItemsList9[0]);
            order9.TotalAmount = orderItemsList9.Sum(x => x.SubTotalAmount);

            var order10 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["10_01_2025"]);
            order10.AddOrderItem(orderItemsList10[0]);
            order10.TotalAmount = orderItemsList10.Sum(x => x.SubTotalAmount);

            var order11 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["11_01_2025"]);
            order11.AddOrderItem(orderItemsList11[0]);
            order11.TotalAmount = orderItemsList11.Sum(x => x.SubTotalAmount);

            var order12 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["12_01_2025"]);
            order12.AddOrderItem(orderItemsList12[0]);
            order12.TotalAmount = orderItemsList12.Sum(x => x.SubTotalAmount);

            var order13 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["13_01_2025"]);
            order13.AddOrderItem(orderItemsList13[0]);
            order13.TotalAmount = orderItemsList13.Sum(x => x.SubTotalAmount);

            var order14 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["14_01_2025"]);
            order14.AddOrderItem(orderItemsList14[0]);
            order14.TotalAmount = orderItemsList14.Sum(x => x.SubTotalAmount);

            var order15 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["15_01_2025"]);
            order15.AddOrderItem(orderItemsList15[0]);
            order15.TotalAmount = orderItemsList15.Sum(x => x.SubTotalAmount);

            var order16 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["16_01_2025"]);
            order16.AddOrderItem(orderItemsList16[0]);
            order16.TotalAmount = orderItemsList16.Sum(x => x.SubTotalAmount);

            var order17 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["17_01_2025"]);
            order17.AddOrderItem(orderItemsList17[0]);
            order17.TotalAmount = orderItemsList17.Sum(x => x.SubTotalAmount);

            var order18 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["18_01_2025"]);
            order18.AddOrderItem(orderItemsList18[0]);
            order18.TotalAmount = orderItemsList18.Sum(x => x.SubTotalAmount);

            var order19 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["19_01_2025"]);
            order19.AddOrderItem(orderItemsList19[0]);
            order19.TotalAmount = orderItemsList19.Sum(x => x.SubTotalAmount);

            var order20 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["20_01_2025"]);
            order20.AddOrderItem(orderItemsList20[0]);
            order20.TotalAmount = orderItemsList20.Sum(x => x.SubTotalAmount);

            var order21 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["21_01_2025"]);
            order21.AddOrderItem(orderItemsList21[0]);
            order21.TotalAmount = orderItemsList21.Sum(x => x.SubTotalAmount);

            var order22 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["22_01_2025"]);
            order22.AddOrderItem(orderItemsList22[0]);
            order22.TotalAmount = orderItemsList22.Sum(x => x.SubTotalAmount);

            var order23 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["23_01_2025"]);
            order23.AddOrderItem(orderItemsList23[0]);
            order23.TotalAmount = orderItemsList23.Sum(x => x.SubTotalAmount);

            var order24 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["24_01_2025"]);
            order24.AddOrderItem(orderItemsList24[0]);
            order24.TotalAmount = orderItemsList24.Sum(x => x.SubTotalAmount);

            var order25 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["25_01_2025"]);
            order25.AddOrderItem(orderItemsList25[0]);
            order25.TotalAmount = orderItemsList25.Sum(x => x.SubTotalAmount);

            var order26 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["26_01_2025"]);
            order26.AddOrderItem(orderItemsList26[0]);
            order26.TotalAmount = orderItemsList26.Sum(x => x.SubTotalAmount);

            var order27 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["27_01_2025"]);
            order27.AddOrderItem(orderItemsList27[0]);
            order27.TotalAmount = orderItemsList27.Sum(x => x.SubTotalAmount);

            var order28 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["28_01_2025"]);
            order28.AddOrderItem(orderItemsList28[0]);
            order28.TotalAmount = orderItemsList28.Sum(x => x.SubTotalAmount);

            var order29 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["29_01_2025"]);
            order29.AddOrderItem(orderItemsList29[0]);
            order29.TotalAmount = orderItemsList29.Sum(x => x.SubTotalAmount);

            var order30 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["30_01_2025"]);
            order30.AddOrderItem(orderItemsList30[0]);
            order30.TotalAmount = orderItemsList30.Sum(x => x.SubTotalAmount);

            var order31 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["31_01_2025"]);
            order31.AddOrderItem(orderItemsList31[0]);
            order31.TotalAmount = orderItemsList31.Sum(x => x.SubTotalAmount);

            var order32 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["01_02_2025"]);
            order32.AddOrderItem(orderItemsList32[0]);
            order32.TotalAmount = orderItemsList32.Sum(x => x.SubTotalAmount);

            var order33 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["02_02_2025"]);
            order33.AddOrderItem(orderItemsList33[0]);
            order33.TotalAmount = orderItemsList33.Sum(x => x.SubTotalAmount);

            var order34 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["03_02_2025"]);
            order34.AddOrderItem(orderItemsList34[0]);
            order34.TotalAmount = orderItemsList34.Sum(x => x.SubTotalAmount);

            var order35 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["04_02_2025"]);
            order35.AddOrderItem(orderItemsList35[0]);
            order35.TotalAmount = orderItemsList35.Sum(x => x.SubTotalAmount);

            var order36 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["05_02_2025"]);
            order36.AddOrderItem(orderItemsList36[0]);
            order36.TotalAmount = orderItemsList36.Sum(x => x.SubTotalAmount);

            var order37 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["06_02_2025"]);
            order37.AddOrderItem(orderItemsList37[0]);
            order37.TotalAmount = orderItemsList37.Sum(x => x.SubTotalAmount);

            var order38 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["07_02_2025"]);
            order38.AddOrderItem(orderItemsList38[0]);
            order38.TotalAmount = orderItemsList38.Sum(x => x.SubTotalAmount);

            var order39 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["08_02_2025"]);
            order39.AddOrderItem(orderItemsList39[0]);
            order39.TotalAmount = orderItemsList39.Sum(x => x.SubTotalAmount);

            var order40 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["09_02_2025"]);
            order40.AddOrderItem(orderItemsList40[0]);
            order40.TotalAmount = orderItemsList40.Sum(x => x.SubTotalAmount);




            // 40 orders of iPhone 16
            var order41 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["10_02_2025"]);
            order41.AddOrderItem(orderItemsList41[0]);
            order41.TotalAmount = orderItemsList41.Sum(x => x.SubTotalAmount);

            var order42 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["11_02_2025"]);
            order42.AddOrderItem(orderItemsList42[0]);
            order42.TotalAmount = orderItemsList42.Sum(x => x.SubTotalAmount);

            var order43 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["12_02_2025"]);
            order43.AddOrderItem(orderItemsList43[0]);
            order43.TotalAmount = orderItemsList43.Sum(x => x.SubTotalAmount);

            var order44 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["13_02_2025"]);
            order44.AddOrderItem(orderItemsList44[0]);
            order44.TotalAmount = orderItemsList44.Sum(x => x.SubTotalAmount);

            var order45 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["14_02_2025"]);
            order45.AddOrderItem(orderItemsList45[0]);
            order45.TotalAmount = orderItemsList45.Sum(x => x.SubTotalAmount);

            var order46 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["15_02_2025"]);
            order46.AddOrderItem(orderItemsList46[0]);
            order46.TotalAmount = orderItemsList46.Sum(x => x.SubTotalAmount);

            var order47 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["16_02_2025"]);
            order47.AddOrderItem(orderItemsList47[0]);
            order47.TotalAmount = orderItemsList47.Sum(x => x.SubTotalAmount);

            var order48 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["17_02_2025"]);
            order48.AddOrderItem(orderItemsList48[0]);
            order48.TotalAmount = orderItemsList48.Sum(x => x.SubTotalAmount);

            var order49 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["18_02_2025"]);
            order49.AddOrderItem(orderItemsList49[0]);
            order49.TotalAmount = orderItemsList49.Sum(x => x.SubTotalAmount);

            var order50 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["19_02_2025"]);
            order50.AddOrderItem(orderItemsList50[0]);
            order50.TotalAmount = orderItemsList50.Sum(x => x.SubTotalAmount);

            var order51 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["20_02_2025"]);
            order51.AddOrderItem(orderItemsList51[0]);
            order51.TotalAmount = orderItemsList51.Sum(x => x.SubTotalAmount);

            var order52 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["21_02_2025"]);
            order52.AddOrderItem(orderItemsList52[0]);
            order52.TotalAmount = orderItemsList52.Sum(x => x.SubTotalAmount);

            var order53 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["22_02_2025"]);
            order53.AddOrderItem(orderItemsList53[0]);
            order53.TotalAmount = orderItemsList53.Sum(x => x.SubTotalAmount);

            var order54 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["23_02_2025"]);
            order54.AddOrderItem(orderItemsList54[0]);
            order54.TotalAmount = orderItemsList54.Sum(x => x.SubTotalAmount);

            var order55 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["24_02_2025"]);
            order55.AddOrderItem(orderItemsList55[0]);
            order55.TotalAmount = orderItemsList55.Sum(x => x.SubTotalAmount);

            var order56 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["25_02_2025"]);
            order56.AddOrderItem(orderItemsList56[0]);
            order56.TotalAmount = orderItemsList56.Sum(x => x.SubTotalAmount);

            var order57 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["26_02_2025"]);
            order57.AddOrderItem(orderItemsList57[0]);
            order57.TotalAmount = orderItemsList57.Sum(x => x.SubTotalAmount);

            var order58 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["27_02_2025"]);
            order58.AddOrderItem(orderItemsList58[0]);
            order58.TotalAmount = orderItemsList58.Sum(x => x.SubTotalAmount);

            var order59 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["28_02_2025"]);
            order59.AddOrderItem(orderItemsList59[0]);
            order59.TotalAmount = orderItemsList59.Sum(x => x.SubTotalAmount);

            var order60 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["01_03_2025"]);
            order60.AddOrderItem(orderItemsList60[0]);
            order60.TotalAmount = orderItemsList60.Sum(x => x.SubTotalAmount);

            var order61 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["02_03_2025"]);
            order61.AddOrderItem(orderItemsList61[0]);
            order61.TotalAmount = orderItemsList61.Sum(x => x.SubTotalAmount);

            var order62 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["03_03_2025"]);
            order62.AddOrderItem(orderItemsList62[0]);
            order62.TotalAmount = orderItemsList62.Sum(x => x.SubTotalAmount);

            var order63 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["04_03_2025"]);
            order63.AddOrderItem(orderItemsList63[0]);
            order63.TotalAmount = orderItemsList63.Sum(x => x.SubTotalAmount);

            var order64 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["05_03_2025"]);
            order64.AddOrderItem(orderItemsList64[0]);
            order64.TotalAmount = orderItemsList64.Sum(x => x.SubTotalAmount);

            var order65 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["06_03_2025"]);
            order65.AddOrderItem(orderItemsList65[0]);
            order65.TotalAmount = orderItemsList65.Sum(x => x.SubTotalAmount);

            var order66 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["07_03_2025"]);
            order66.AddOrderItem(orderItemsList66[0]);
            order66.TotalAmount = orderItemsList66.Sum(x => x.SubTotalAmount);

            var order67 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["08_03_2025"]);
            order67.AddOrderItem(orderItemsList67[0]);
            order67.TotalAmount = orderItemsList67.Sum(x => x.SubTotalAmount);

            var order68 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["09_03_2025"]);
            order68.AddOrderItem(orderItemsList68[0]);
            order68.TotalAmount = orderItemsList68.Sum(x => x.SubTotalAmount);

            var order69 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["10_03_2025"]);
            order69.AddOrderItem(orderItemsList69[0]);
            order69.TotalAmount = orderItemsList69.Sum(x => x.SubTotalAmount);

            var order70 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["11_03_2025"]);
            order70.AddOrderItem(orderItemsList70[0]);
            order70.TotalAmount = orderItemsList70.Sum(x => x.SubTotalAmount);

            var order71 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["12_03_2025"]);
            order71.AddOrderItem(orderItemsList71[0]);
            order71.TotalAmount = orderItemsList71.Sum(x => x.SubTotalAmount);

            var order72 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["13_03_2025"]);
            order72.AddOrderItem(orderItemsList72[0]);
            order72.TotalAmount = orderItemsList72.Sum(x => x.SubTotalAmount);

            var order73 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["14_03_2025"]);
            order73.AddOrderItem(orderItemsList73[0]);
            order73.TotalAmount = orderItemsList73.Sum(x => x.SubTotalAmount);

            var order74 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["15_03_2025"]);
            order74.AddOrderItem(orderItemsList74[0]);
            order74.TotalAmount = orderItemsList74.Sum(x => x.SubTotalAmount);

            var order75 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["16_03_2025"]);
            order75.AddOrderItem(orderItemsList75[0]);
            order75.TotalAmount = orderItemsList75.Sum(x => x.SubTotalAmount);

            var order76 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["17_03_2025"]);
            order76.AddOrderItem(orderItemsList76[0]);
            order76.TotalAmount = orderItemsList76.Sum(x => x.SubTotalAmount);

            var order77 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["18_03_2025"]);
            order77.AddOrderItem(orderItemsList77[0]);
            order77.TotalAmount = orderItemsList77.Sum(x => x.SubTotalAmount);

            var order78 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["19_03_2025"]);
            order78.AddOrderItem(orderItemsList78[0]);
            order78.TotalAmount = orderItemsList78.Sum(x => x.SubTotalAmount);

            var order79 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["20_03_2025"]);
            order79.AddOrderItem(orderItemsList79[0]);
            order79.TotalAmount = orderItemsList79.Sum(x => x.SubTotalAmount);

            var order80 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["21_03_2025"]);
            order80.AddOrderItem(orderItemsList80[0]);
            order80.TotalAmount = orderItemsList80.Sum(x => x.SubTotalAmount);


            // 8 orders of iPhone 16e
            var order81 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["22_03_2025"]);
            order81.AddOrderItem(orderItemsList81[0]);
            order81.TotalAmount = orderItemsList81.Sum(x => x.SubTotalAmount);

            var order82 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["23_03_2025"]);
            order82.AddOrderItem(orderItemsList82[0]);
            order82.TotalAmount = orderItemsList82.Sum(x => x.SubTotalAmount);

            var order83 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["24_03_2025"]);
            order83.AddOrderItem(orderItemsList83[0]);
            order83.TotalAmount = orderItemsList83.Sum(x => x.SubTotalAmount);

            var order84 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["25_03_2025"]);
            order84.AddOrderItem(orderItemsList84[0]);
            order84.TotalAmount = orderItemsList84.Sum(x => x.SubTotalAmount);

            var order85 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["26_03_2025"]);
            order85.AddOrderItem(orderItemsList85[0]);
            order85.TotalAmount = orderItemsList85.Sum(x => x.SubTotalAmount);

            var order86 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["27_03_2025"]);
            order86.AddOrderItem(orderItemsList86[0]);
            order86.TotalAmount = orderItemsList86.Sum(x => x.SubTotalAmount);

            var order87 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["28_03_2025"]);
            order87.AddOrderItem(orderItemsList87[0]);
            order87.TotalAmount = orderItemsList87.Sum(x => x.SubTotalAmount);

            var order88 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["29_03_2025"]);
            order88.AddOrderItem(orderItemsList88[0]);
            order88.TotalAmount = orderItemsList88.Sum(x => x.SubTotalAmount);


            // 20 orders of iPhone 17
            var order89 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["30_03_2025"]);
            order89.AddOrderItem(orderItemsList89[0]);
            order89.TotalAmount = orderItemsList89.Sum(x => x.SubTotalAmount);

            var order90 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["31_03_2025"]);
            order90.AddOrderItem(orderItemsList90[0]);
            order90.TotalAmount = orderItemsList90.Sum(x => x.SubTotalAmount);

            var order91 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["01_04_2025"]);
            order91.AddOrderItem(orderItemsList91[0]);
            order91.TotalAmount = orderItemsList91.Sum(x => x.SubTotalAmount);

            var order92 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["02_04_2025"]);
            order92.AddOrderItem(orderItemsList92[0]);
            order92.TotalAmount = orderItemsList92.Sum(x => x.SubTotalAmount);

            var order93 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["03_04_2025"]);
            order93.AddOrderItem(orderItemsList93[0]);
            order93.TotalAmount = orderItemsList93.Sum(x => x.SubTotalAmount);

            var order94 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["04_04_2025"]);
            order94.AddOrderItem(orderItemsList94[0]);
            order94.TotalAmount = orderItemsList94.Sum(x => x.SubTotalAmount);

            var order95 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["05_04_2025"]);
            order95.AddOrderItem(orderItemsList95[0]);
            order95.TotalAmount = orderItemsList95.Sum(x => x.SubTotalAmount);

            var order96 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["06_04_2025"]);
            order96.AddOrderItem(orderItemsList96[0]);
            order96.TotalAmount = orderItemsList96.Sum(x => x.SubTotalAmount);

            var order97 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["07_04_2025"]);
            order97.AddOrderItem(orderItemsList97[0]);
            order97.TotalAmount = orderItemsList97.Sum(x => x.SubTotalAmount);

            var order98 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["08_04_2025"]);
            order98.AddOrderItem(orderItemsList98[0]);
            order98.TotalAmount = orderItemsList98.Sum(x => x.SubTotalAmount);

            var order99 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["09_04_2025"]);
            order99.AddOrderItem(orderItemsList99[0]);
            order99.TotalAmount = orderItemsList99.Sum(x => x.SubTotalAmount);

            var order100 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["10_04_2025"]);
            order100.AddOrderItem(orderItemsList100[0]);
            order100.TotalAmount = orderItemsList100.Sum(x => x.SubTotalAmount);

            var order101 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["11_04_2025"]);
            order101.AddOrderItem(orderItemsList101[0]);
            order101.TotalAmount = orderItemsList101.Sum(x => x.SubTotalAmount);

            var order102 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["12_04_2025"]);
            order102.AddOrderItem(orderItemsList102[0]);
            order102.TotalAmount = orderItemsList102.Sum(x => x.SubTotalAmount);

            var order103 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["13_04_2025"]);
            order103.AddOrderItem(orderItemsList103[0]);
            order103.TotalAmount = orderItemsList103.Sum(x => x.SubTotalAmount);

            var order104 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["14_04_2025"]);
            order104.AddOrderItem(orderItemsList104[0]);
            order104.TotalAmount = orderItemsList104.Sum(x => x.SubTotalAmount);

            var order105 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["15_04_2025"]);
            order105.AddOrderItem(orderItemsList105[0]);
            order105.TotalAmount = orderItemsList105.Sum(x => x.SubTotalAmount);

            var order106 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["16_04_2025"]);
            order106.AddOrderItem(orderItemsList106[0]);
            order106.TotalAmount = orderItemsList106.Sum(x => x.SubTotalAmount);

            var order107 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["17_04_2025"]);
            order107.AddOrderItem(orderItemsList107[0]);
            order107.TotalAmount = orderItemsList107.Sum(x => x.SubTotalAmount);

            var order108 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["18_04_2025"]);
            order108.AddOrderItem(orderItemsList108[0]);
            order108.TotalAmount = orderItemsList108.Sum(x => x.SubTotalAmount);


            // 24 orders of iPhone 17 Pro
            var order109 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["19_04_2025"]);
            order109.AddOrderItem(orderItemsList109[0]);
            order109.TotalAmount = orderItemsList109.Sum(x => x.SubTotalAmount);

            var order110 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["20_04_2025"]);
            order110.AddOrderItem(orderItemsList110[0]);
            order110.TotalAmount = orderItemsList110.Sum(x => x.SubTotalAmount);

            var order111 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["21_04_2025"]);
            order111.AddOrderItem(orderItemsList111[0]);
            order111.TotalAmount = orderItemsList111.Sum(x => x.SubTotalAmount);

            var order112 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["22_04_2025"]);
            order112.AddOrderItem(orderItemsList112[0]);
            order112.TotalAmount = orderItemsList112.Sum(x => x.SubTotalAmount);

            var order113 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["23_04_2025"]);
            order113.AddOrderItem(orderItemsList113[0]);
            order113.TotalAmount = orderItemsList113.Sum(x => x.SubTotalAmount);

            var order114 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["24_04_2025"]);
            order114.AddOrderItem(orderItemsList114[0]);
            order114.TotalAmount = orderItemsList114.Sum(x => x.SubTotalAmount);

            var order115 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["25_04_2025"]);
            order115.AddOrderItem(orderItemsList115[0]);
            order115.TotalAmount = orderItemsList115.Sum(x => x.SubTotalAmount);

            var order116 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["26_04_2025"]);
            order116.AddOrderItem(orderItemsList116[0]);
            order116.TotalAmount = orderItemsList116.Sum(x => x.SubTotalAmount);

            var order117 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["27_04_2025"]);
            order117.AddOrderItem(orderItemsList117[0]);
            order117.TotalAmount = orderItemsList117.Sum(x => x.SubTotalAmount);

            var order118 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["28_04_2025"]);
            order118.AddOrderItem(orderItemsList118[0]);
            order118.TotalAmount = orderItemsList118.Sum(x => x.SubTotalAmount);

            var order119 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["29_04_2025"]);
            order119.AddOrderItem(orderItemsList119[0]);
            order119.TotalAmount = orderItemsList119.Sum(x => x.SubTotalAmount);

            var order120 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["30_04_2025"]);
            order120.AddOrderItem(orderItemsList120[0]);
            order120.TotalAmount = orderItemsList120.Sum(x => x.SubTotalAmount);

            var order121 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["01_05_2025"]);
            order121.AddOrderItem(orderItemsList121[0]);
            order121.TotalAmount = orderItemsList121.Sum(x => x.SubTotalAmount);

            var order122 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["02_05_2025"]);
            order122.AddOrderItem(orderItemsList122[0]);
            order122.TotalAmount = orderItemsList122.Sum(x => x.SubTotalAmount);

            var order123 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["03_05_2025"]);
            order123.AddOrderItem(orderItemsList123[0]);
            order123.TotalAmount = orderItemsList123.Sum(x => x.SubTotalAmount);

            var order124 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["04_05_2025"]);
            order124.AddOrderItem(orderItemsList124[0]);
            order124.TotalAmount = orderItemsList124.Sum(x => x.SubTotalAmount);

            var order125 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["05_05_2025"]);
            order125.AddOrderItem(orderItemsList125[0]);
            order125.TotalAmount = orderItemsList125.Sum(x => x.SubTotalAmount);

            var order126 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["06_05_2025"]);
            order126.AddOrderItem(orderItemsList126[0]);
            order126.TotalAmount = orderItemsList126.Sum(x => x.SubTotalAmount);

            var order127 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["07_05_2025"]);
            order127.AddOrderItem(orderItemsList127[0]);
            order127.TotalAmount = orderItemsList127.Sum(x => x.SubTotalAmount);

            var order128 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["08_05_2025"]);
            order128.AddOrderItem(orderItemsList128[0]);
            order128.TotalAmount = orderItemsList128.Sum(x => x.SubTotalAmount);

            var order129 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["09_05_2025"]);
            order129.AddOrderItem(orderItemsList129[0]);
            order129.TotalAmount = orderItemsList129.Sum(x => x.SubTotalAmount);

            var order130 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["10_05_2025"]);
            order130.AddOrderItem(orderItemsList130[0]);
            order130.TotalAmount = orderItemsList130.Sum(x => x.SubTotalAmount);

            var order131 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["11_05_2025"]);
            order131.AddOrderItem(orderItemsList131[0]);
            order131.TotalAmount = orderItemsList131.Sum(x => x.SubTotalAmount);

            var order132 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["12_05_2025"]);
            order132.AddOrderItem(orderItemsList132[0]);
            order132.TotalAmount = orderItemsList132.Sum(x => x.SubTotalAmount);


            // 16 orders of iPhone 17 Air
            var order133 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["13_05_2025"]);
            order133.AddOrderItem(orderItemsList133[0]);
            order133.TotalAmount = orderItemsList133.Sum(x => x.SubTotalAmount);

            var order134 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["14_05_2025"]);
            order134.AddOrderItem(orderItemsList134[0]);
            order134.TotalAmount = orderItemsList134.Sum(x => x.SubTotalAmount);

            var order135 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["15_05_2025"]);
            order135.AddOrderItem(orderItemsList135[0]);
            order135.TotalAmount = orderItemsList135.Sum(x => x.SubTotalAmount);

            var order136 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["16_05_2025"]);
            order136.AddOrderItem(orderItemsList136[0]);
            order136.TotalAmount = orderItemsList136.Sum(x => x.SubTotalAmount);

            var order137 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["17_05_2025"]);
            order137.AddOrderItem(orderItemsList137[0]);
            order137.TotalAmount = orderItemsList137.Sum(x => x.SubTotalAmount);

            var order138 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["18_05_2025"]);
            order138.AddOrderItem(orderItemsList138[0]);
            order138.TotalAmount = orderItemsList138.Sum(x => x.SubTotalAmount);

            var order139 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["19_05_2025"]);
            order139.AddOrderItem(orderItemsList139[0]);
            order139.TotalAmount = orderItemsList139.Sum(x => x.SubTotalAmount);

            var order140 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["20_05_2025"]);
            order140.AddOrderItem(orderItemsList140[0]);
            order140.TotalAmount = orderItemsList140.Sum(x => x.SubTotalAmount);

            var order141 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["21_05_2025"]);
            order141.AddOrderItem(orderItemsList141[0]);
            order141.TotalAmount = orderItemsList141.Sum(x => x.SubTotalAmount);

            var order142 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["22_05_2025"]);
            order142.AddOrderItem(orderItemsList142[0]);
            order142.TotalAmount = orderItemsList142.Sum(x => x.SubTotalAmount);

            var order143 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["23_05_2025"]);
            order143.AddOrderItem(orderItemsList143[0]);
            order143.TotalAmount = orderItemsList143.Sum(x => x.SubTotalAmount);

            var order144 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["24_05_2025"]);
            order144.AddOrderItem(orderItemsList144[0]);
            order144.TotalAmount = orderItemsList144.Sum(x => x.SubTotalAmount);

            var order145 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["25_05_2025"]);
            order145.AddOrderItem(orderItemsList145[0]);
            order145.TotalAmount = orderItemsList145.Sum(x => x.SubTotalAmount);

            var order146 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["26_05_2025"]);
            order146.AddOrderItem(orderItemsList146[0]);
            order146.TotalAmount = orderItemsList146.Sum(x => x.SubTotalAmount);

            var order147 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["27_05_2025"]);
            order147.AddOrderItem(orderItemsList147[0]);
            order147.TotalAmount = orderItemsList147.Sum(x => x.SubTotalAmount);

            var order148 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: null,
                                       customerId: listUserIds[0],
                                       customerPublicKey: null,
                                       tx: null,
                                       code: Code.GenerateCode(),
                                       paymentMethod: EPaymentMethod.COD,
                                       orderStatus: EOrderStatus.DELIVERED,
                                       shippingAddress: listShippingAddresses[0],
                                       promotionId: null,
                                       promotionType: null,
                                       discountType: null,
                                       discountValue: null,
                                       discountAmount: null,
                                       createdAt: CreatedAtDateTimes["28_05_2025"]);
            order148.AddOrderItem(orderItemsList148[0]);
            order148.TotalAmount = orderItemsList148.Sum(x => x.SubTotalAmount);

            return new List<Order>
            {
                order1,
                order2,
                order3,
                order4,
                order5,
                order6,
                order7,
                order8,
                order9,
                order10,
                order11,
                order12,
                order13,
                order14,
                order15,
                order16,
                order17,
                order18,
                order19,
                order20,
                order21,
                order22,
                order23,
                order24,
                order25,
                order26,
                order27,
                order28,
                order29,
                order30,
                order31,
                order32,
                order33,
                order34,
                order35,
                order36,
                order37,
                order38,
                order39,
                order40,
                order41,
                order42,
                order43,
                order44,
                order45,
                order46,
                order47,
                order48,
                order49,
                order50,
                order51,
                order52,
                order53,
                order54,
                order55,
                order56,
                order57,
                order58,
                order59,
                order60,
                order61,
                order62,
                order63,
                order64,
                order65,
                order66,
                order67,
                order68,
                order69,
                order70,
                order71,
                order72,
                order73,
                order74,
                order75,
                order76,
                order77,
                order78,
                order79,
                order80,
                order81,
                order82,
                order83,
                order84,
                order85,
                order86,
                order87,
                order88,
                order89,
                order90,
                order91,
                order92,
                order93,
                order94,
                order95,
                order96,
                order97,
                order98,
                order99,
                order100,
                order101,
                order102,
                order103,
                order104,
                order105,
                order106,
                order107,
                order108,
                order109,
                order110,
                order111,
                order112,
                order113,
                order114,
                order115,
                order116,
                order117,
                order118,
                order119,
                order120,
                order121,
                order122,
                order123,
                order124,
                order125,
                order126,
                order127,
                order128,
                order129,
                order130,
                order131,
                order132,
                order133,
                order134,
                order135,
                order136,
                order137,
                order138,
                order139,
                order140,
                order141,
                order142,
                order143,
                order144,
                order145,
                order146,
                order147,
                order148
            };
        }
    }

    public static Dictionary<string, string> ModelIds => new Dictionary<string, string>
    {
        { "IPHONE_15", "664351e90087aa09993f5ae7" },
        { "IPHONE_16", "6643543d0087aa09993f5b14" },
        { "IPHONE_16E", "6643543e0087aa09993f5b15" },
        { "IPHONE_17", "6643543f0087aa09993f5b16" },
        { "IPHONE_17_PRO", "664354400087aa09993f5b17" },
        { "IPHONE_17_AIR", "664354410087aa09993f5b18" }
    };

    public static IEnumerable<string> iPhone15Models => new List<string>
            {
                "IPHONE_15",
                "IPHONE_15_PLUS",
            };

    public static IEnumerable<string> iPhone16Models => new List<string>
            {
                "IPHONE_16",
                "IPHONE_16_PLUS",
            };

    public static IEnumerable<string> iPhone16EModels => new List<string>
            {
                "IPHONE_16E",
            };

    public static IEnumerable<string> iPhone17Models => new List<string>
            {
                "IPHONE_17",
            };

    public static IEnumerable<string> iPhone17ProModels => new List<string>
            {
                "IPHONE_17_PRO",
                "IPHONE_17_PRO_MAX",
            };

    public static IEnumerable<string> iPhone17AirModels => new List<string>
            {
                "IPHONE_17_AIR",
            };

    public static IEnumerable<string> iPhone15Colors => new List<string>
            {
                "BLUE",
                "PINK",
                "YELLOW",
                "GREEN",
                "BLACK",
            };

    public static IEnumerable<string> iPhone16Colors => new List<string>
            {
                "ULTRAMARINE",
                "TEAL",
                "PINK",
                "WHITE",
                "BLACK",
            };

    public static IEnumerable<string> iPhone16EColors => new List<string>
            {
                "WHITE",
                "BLACK",
            };

    public static IEnumerable<string> iPhone17Colors => new List<string>
            {
                "LAVENDER",
                "SAGE",
                "MIST_BLUE",
                "WHITE",
                "BLACK",
            };

    public static IEnumerable<string> iPhone17ProColors => new List<string>
            {
                "SILVER",
                "COSMIC_ORANGE",
                "DEEP_BLUE",
            };

    public static IEnumerable<string> iPhone17AirColors => new List<string>
            {
                "SKY_BLUE",
                "LIGHT_GOLD",
                "CLOUD_WHITE",
                "SPACE_BLACK",
            };

    public static IEnumerable<string> Storages => new List<string>
            {
                "128GB",
                "256GB",
                "512GB",
                "1TB",
            };

    public static IEnumerable<string> ModelSlugs => new List<string>
    {
        "iphone-15",
        "iphone-16",
        "iphone-16e",
        "iphone-17",
        "iphone-17-pro",
        "iphone-17-air",
    };

    public static IEnumerable<string> SkuIdsInWarehouse => new List<string>
    {
        "690f4601e2295b9f94f23f5f", // IPHONE-IPHONE_15-128GB-BLUE
    };

    public static IEnumerable<SkuPriceListResponse> SkuPriceList
    {
        get
        {
            var prices = new List<SkuPriceListResponse>();

            // Use the static IEnumerable properties
            var iPhone15ModelsList = iPhone15Models.ToList();
            var iPhone16ModelsList = iPhone16Models.ToList();
            var iPhone16EModelsList = iPhone16EModels.ToList();
            var iPhone17ModelsList = iPhone17Models.ToList();
            var iPhone17ProModelsList = iPhone17ProModels.ToList();
            var iPhone17AirModelsList = iPhone17AirModels.ToList();

            var iPhone15ColorsList = iPhone15Colors.ToList();
            var iPhone16ColorsList = iPhone16Colors.ToList();
            var iPhone16EColorsList = iPhone16EColors.ToList();
            var iPhone17ColorsList = iPhone17Colors.ToList();
            var iPhone17ProColorsList = iPhone17ProColors.ToList();
            var iPhone17AirColorsList = iPhone17AirColors.ToList();

            var StoragesList = Storages.ToList();

            // Base prices for each model series
            var basePrices = new Dictionary<string, decimal>
            {
                { "IPHONE_15", 1000 },
                { "IPHONE_15_PLUS", 1100 },
                { "IPHONE_16", 1200 },
                { "IPHONE_16_PLUS", 1300 },
                { "IPHONE_16E", 1000 },
                { "IPHONE_17", 1500 },
                { "IPHONE_17_PRO", 1800 },
                { "IPHONE_17_PRO_MAX", 2000 },
                { "IPHONE_17_AIR", 1400 }
            };

            // Storage price increments
            var storageIncrements = new Dictionary<string, decimal>
            {
                { "128GB", 0 },
                { "256GB", 100 },
                { "512GB", 200 },
                { "1TB", 300 }
            };

            // Counter for generating unique SKU IDs
            var skuIdCounter = 0;

            // Helper function to generate prices for a model group
            void GeneratePricesForModelGroup(List<string> models, List<string> colors, decimal basePriceOffset = 0)
            {
                foreach (var model in models)
                {
                    var modelBasePrice = basePrices.GetValueOrDefault(model, 1000) + basePriceOffset;
                    foreach (var color in colors)
                    {
                        foreach (var storage in StoragesList)
                        {
                            var storageIncrement = storageIncrements.GetValueOrDefault(storage, 0);
                            var unitPrice = modelBasePrice + storageIncrement;

                            // Normalize model, color, and storage using enums
                            EIphoneModel.TryFromName(model, out var modelEnum);
                            EColor.TryFromName(color, out var colorEnum);
                            EStorage.TryFromName(storage, out var storageEnum);

                            // Generate a unique SKU ID (placeholder - will be replaced with actual SKU ID when creating orders)
                            var skuId = $"seed-sku-{++skuIdCounter:D6}";

                            prices.Add(new SkuPriceListResponse
                            {
                                SkuId = skuId,
                                NormalizedModel = modelEnum?.Name ?? model,
                                NormalizedColor = colorEnum?.Name ?? color,
                                NormalizedStorage = storageEnum?.Name ?? storage,
                                UnitPrice = unitPrice
                            });
                        }
                    }
                }
            }

            // Generate prices for iPhone 15 series
            GeneratePricesForModelGroup(iPhone15ModelsList, iPhone15ColorsList);

            // Generate prices for iPhone 16 series
            GeneratePricesForModelGroup(iPhone16ModelsList, iPhone16ColorsList);

            // Generate prices for iPhone 16E series
            GeneratePricesForModelGroup(iPhone16EModelsList, iPhone16EColorsList);

            // Generate prices for iPhone 17 series
            GeneratePricesForModelGroup(iPhone17ModelsList, iPhone17ColorsList);

            // Generate prices for iPhone 17 Pro series
            GeneratePricesForModelGroup(iPhone17ProModelsList, iPhone17ProColorsList);

            // Generate prices for iPhone 17 Air series
            GeneratePricesForModelGroup(iPhone17AirModelsList, iPhone17AirColorsList);

            return prices;
        }
    }

    private static string GetModelSlug(string modelName)
    {
        var modelSlugs = ModelSlugs.ToList();

        if (modelName.StartsWith("IPHONE_15"))
            return modelSlugs[0]; // "iphone-15"
        if (modelName.StartsWith("IPHONE_16") && !modelName.Contains("E"))
            return modelSlugs[1]; // "iphone-16"
        if (modelName.StartsWith("IPHONE_16E"))
            return modelSlugs[2]; // "iphone-16e"
        if (modelName.StartsWith("IPHONE_17") && !modelName.Contains("PRO") && !modelName.Contains("AIR"))
            return modelSlugs[3]; // "iphone-17"
        if (modelName.StartsWith("IPHONE_17_PRO"))
            return modelSlugs[4]; // "iphone-17-pro"
        if (modelName.StartsWith("IPHONE_17_AIR"))
            return modelSlugs[5]; // "iphone-17-air"

        return modelSlugs.First();
    }

    // Helper method to get price from SkuPriceList
    private static decimal GetPriceFromSkuPriceList(string model, string color, string storage)
    {
        // Normalize the values using enums
        EIphoneModel.TryFromName(model, out var modelEnum);
        EColor.TryFromName(color, out var colorEnum);
        EStorage.TryFromName(storage, out var storageEnum);

        var normalizedModel = modelEnum?.Name ?? model;
        var normalizedColor = colorEnum?.Name ?? color;
        var normalizedStorage = storageEnum?.Name ?? storage;

        // Find matching price in SkuPriceList
        var price = SkuPriceList.FirstOrDefault(p =>
            p.NormalizedModel == normalizedModel &&
            p.NormalizedColor == normalizedColor &&
            p.NormalizedStorage == normalizedStorage);

        return price?.UnitPrice ?? 0;
    }

    public static Dictionary<string, DateTime> CreatedAtDateTimes => new Dictionary<string, DateTime>
    {
        { "01_01_2025", new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 1
        { "02_01_2025", new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 2
        { "03_01_2025", new DateTime(2025, 1, 3, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 3
        { "04_01_2025", new DateTime(2025, 1, 4, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 4
        { "05_01_2025", new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 5
        { "06_01_2025", new DateTime(2025, 1, 6, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 6
        { "07_01_2025", new DateTime(2025, 1, 7, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 7
        { "08_01_2025", new DateTime(2025, 1, 8, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 8
        { "09_01_2025", new DateTime(2025, 1, 9, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 9
        { "10_01_2025", new DateTime(2025, 1, 10, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 10
        { "11_01_2025", new DateTime(2025, 1, 11, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 11
        { "12_01_2025", new DateTime(2025, 1, 12, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 12
        { "13_01_2025", new DateTime(2025, 1, 13, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 13
        { "14_01_2025", new DateTime(2025, 1, 14, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 14
        { "15_01_2025", new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 15
        { "16_01_2025", new DateTime(2025, 1, 16, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 16
        { "17_01_2025", new DateTime(2025, 1, 17, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 17
        { "18_01_2025", new DateTime(2025, 1, 18, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 18
        { "19_01_2025", new DateTime(2025, 1, 19, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 19
        { "20_01_2025", new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 20
        { "21_01_2025", new DateTime(2025, 1, 21, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 21
        { "22_01_2025", new DateTime(2025, 1, 22, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 22
        { "23_01_2025", new DateTime(2025, 1, 23, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 23
        { "24_01_2025", new DateTime(2025, 1, 24, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 24
        { "25_01_2025", new DateTime(2025, 1, 25, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 25
        { "26_01_2025", new DateTime(2025, 1, 26, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 26
        { "27_01_2025", new DateTime(2025, 1, 27, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 27
        { "28_01_2025", new DateTime(2025, 1, 28, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 28
        { "29_01_2025", new DateTime(2025, 1, 29, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 29
        { "30_01_2025", new DateTime(2025, 1, 30, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 30
        { "31_01_2025", new DateTime(2025, 1, 31, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 31
        { "01_02_2025", new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 1
        { "02_02_2025", new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 2
        { "03_02_2025", new DateTime(2025, 2, 3, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 3
        { "04_02_2025", new DateTime(2025, 2, 4, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 4
        { "05_02_2025", new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 5
        { "06_02_2025", new DateTime(2025, 2, 6, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 6
        { "07_02_2025", new DateTime(2025, 2, 7, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 7
        { "08_02_2025", new DateTime(2025, 2, 8, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 8
        { "09_02_2025", new DateTime(2025, 2, 9, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 9
        { "10_02_2025", new DateTime(2025, 2, 10, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 10
        { "11_02_2025", new DateTime(2025, 2, 11, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 11
        { "12_02_2025", new DateTime(2025, 2, 12, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 12
        { "13_02_2025", new DateTime(2025, 2, 13, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 13
        { "14_02_2025", new DateTime(2025, 2, 14, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 14
        { "15_02_2025", new DateTime(2025, 2, 15, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 15
        { "16_02_2025", new DateTime(2025, 2, 16, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 16
        { "17_02_2025", new DateTime(2025, 2, 17, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 17
        { "18_02_2025", new DateTime(2025, 2, 18, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 18
        { "19_02_2025", new DateTime(2025, 2, 19, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 19
        { "20_02_2025", new DateTime(2025, 2, 20, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 20
        { "21_02_2025", new DateTime(2025, 2, 21, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 21
        { "22_02_2025", new DateTime(2025, 2, 22, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 22
        { "23_02_2025", new DateTime(2025, 2, 23, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 23
        { "24_02_2025", new DateTime(2025, 2, 24, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 24
        { "25_02_2025", new DateTime(2025, 2, 25, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 25
        { "26_02_2025", new DateTime(2025, 2, 26, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 26
        { "27_02_2025", new DateTime(2025, 2, 27, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 27
        { "28_02_2025", new DateTime(2025, 2, 28, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 28
        { "01_03_2025", new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 1
        { "02_03_2025", new DateTime(2025, 3, 2, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 2
        { "03_03_2025", new DateTime(2025, 3, 3, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 3
        { "04_03_2025", new DateTime(2025, 3, 4, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 4
        { "05_03_2025", new DateTime(2025, 3, 5, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 5
        { "06_03_2025", new DateTime(2025, 3, 6, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 6
        { "07_03_2025", new DateTime(2025, 3, 7, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 7
        { "08_03_2025", new DateTime(2025, 3, 8, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 8
        { "09_03_2025", new DateTime(2025, 3, 9, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 9
        { "10_03_2025", new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 10
        { "11_03_2025", new DateTime(2025, 3, 11, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 11
        { "12_03_2025", new DateTime(2025, 3, 12, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 12
        { "13_03_2025", new DateTime(2025, 3, 13, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 13
        { "14_03_2025", new DateTime(2025, 3, 14, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 14
        { "15_03_2025", new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 15
        { "16_03_2025", new DateTime(2025, 3, 16, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 16
        { "17_03_2025", new DateTime(2025, 3, 17, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 17
        { "18_03_2025", new DateTime(2025, 3, 18, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 18
        { "19_03_2025", new DateTime(2025, 3, 19, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 19
        { "20_03_2025", new DateTime(2025, 3, 20, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 20
        { "21_03_2025", new DateTime(2025, 3, 21, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 21
        { "22_03_2025", new DateTime(2025, 3, 22, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 22
        { "23_03_2025", new DateTime(2025, 3, 23, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 23
        { "24_03_2025", new DateTime(2025, 3, 24, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 24
        { "25_03_2025", new DateTime(2025, 3, 25, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 25
        { "26_03_2025", new DateTime(2025, 3, 26, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 26
        { "27_03_2025", new DateTime(2025, 3, 27, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 27
        { "28_03_2025", new DateTime(2025, 3, 28, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 28
        { "29_03_2025", new DateTime(2025, 3, 29, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 29
        { "30_03_2025", new DateTime(2025, 3, 30, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 30
        { "31_03_2025", new DateTime(2025, 3, 31, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 31
        { "01_04_2025", new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 1
        { "02_04_2025", new DateTime(2025, 4, 2, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 2
        { "03_04_2025", new DateTime(2025, 4, 3, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 3
        { "04_04_2025", new DateTime(2025, 4, 4, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 4
        { "05_04_2025", new DateTime(2025, 4, 5, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 5
        { "06_04_2025", new DateTime(2025, 4, 6, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 6
        { "07_04_2025", new DateTime(2025, 4, 7, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 7
        { "08_04_2025", new DateTime(2025, 4, 8, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 8
        { "09_04_2025", new DateTime(2025, 4, 9, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 9
        { "10_04_2025", new DateTime(2025, 4, 10, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 10
        { "11_04_2025", new DateTime(2025, 4, 11, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 11
        { "12_04_2025", new DateTime(2025, 4, 12, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 12
        { "13_04_2025", new DateTime(2025, 4, 13, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 13
        { "14_04_2025", new DateTime(2025, 4, 14, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 14
        { "15_04_2025", new DateTime(2025, 4, 15, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 15
        { "16_04_2025", new DateTime(2025, 4, 16, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 16
        { "17_04_2025", new DateTime(2025, 4, 17, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 17
        { "18_04_2025", new DateTime(2025, 4, 18, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 18
        { "19_04_2025", new DateTime(2025, 4, 19, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 19
        { "20_04_2025", new DateTime(2025, 4, 20, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 20
        { "21_04_2025", new DateTime(2025, 4, 21, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 21
        { "22_04_2025", new DateTime(2025, 4, 22, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 22
        { "23_04_2025", new DateTime(2025, 4, 23, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 23
        { "24_04_2025", new DateTime(2025, 4, 24, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 24
        { "25_04_2025", new DateTime(2025, 4, 25, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 25
        { "26_04_2025", new DateTime(2025, 4, 26, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 26
        { "27_04_2025", new DateTime(2025, 4, 27, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 27
        { "28_04_2025", new DateTime(2025, 4, 28, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 28
        { "29_04_2025", new DateTime(2025, 4, 29, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 29
        { "30_04_2025", new DateTime(2025, 4, 30, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 30
        { "01_05_2025", new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 1
        { "02_05_2025", new DateTime(2025, 5, 2, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 2
        { "03_05_2025", new DateTime(2025, 5, 3, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 3
        { "04_05_2025", new DateTime(2025, 5, 4, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 4
        { "05_05_2025", new DateTime(2025, 5, 5, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 5
        { "06_05_2025", new DateTime(2025, 5, 6, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 6
        { "07_05_2025", new DateTime(2025, 5, 7, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 7
        { "08_05_2025", new DateTime(2025, 5, 8, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 8
        { "09_05_2025", new DateTime(2025, 5, 9, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 9
        { "10_05_2025", new DateTime(2025, 5, 10, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 10
        { "11_05_2025", new DateTime(2025, 5, 11, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 11
        { "12_05_2025", new DateTime(2025, 5, 12, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 12
        { "13_05_2025", new DateTime(2025, 5, 13, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 13
        { "14_05_2025", new DateTime(2025, 5, 14, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 14
        { "15_05_2025", new DateTime(2025, 5, 15, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 15
        { "16_05_2025", new DateTime(2025, 5, 16, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 16
        { "17_05_2025", new DateTime(2025, 5, 17, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 17
        { "18_05_2025", new DateTime(2025, 5, 18, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 18
        { "19_05_2025", new DateTime(2025, 5, 19, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 19
        { "20_05_2025", new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 20
        { "21_05_2025", new DateTime(2025, 5, 21, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 21
        { "22_05_2025", new DateTime(2025, 5, 22, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 22
        { "23_05_2025", new DateTime(2025, 5, 23, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 23
        { "24_05_2025", new DateTime(2025, 5, 24, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 24
        { "25_05_2025", new DateTime(2025, 5, 25, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 25
        { "26_05_2025", new DateTime(2025, 5, 26, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 26
        { "27_05_2025", new DateTime(2025, 5, 27, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 27
        { "28_05_2025", new DateTime(2025, 5, 28, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 28
        { "29_05_2025", new DateTime(2025, 5, 29, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 29
        { "30_05_2025", new DateTime(2025, 5, 30, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 30
        { "31_05_2025", new DateTime(2025, 5, 31, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 31
        { "01_06_2025", new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 1
        { "02_06_2025", new DateTime(2025, 6, 2, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 2
        { "03_06_2025", new DateTime(2025, 6, 3, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 3
        { "04_06_2025", new DateTime(2025, 6, 4, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 4
        { "05_06_2025", new DateTime(2025, 6, 5, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 5
        { "06_06_2025", new DateTime(2025, 6, 6, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 6
        { "07_06_2025", new DateTime(2025, 6, 7, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 7
        { "08_06_2025", new DateTime(2025, 6, 8, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 8
        { "09_06_2025", new DateTime(2025, 6, 9, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 9
        { "10_06_2025", new DateTime(2025, 6, 10, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 10
        { "11_06_2025", new DateTime(2025, 6, 11, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 11
        { "12_06_2025", new DateTime(2025, 6, 12, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 12
        { "13_06_2025", new DateTime(2025, 6, 13, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 13
        { "14_06_2025", new DateTime(2025, 6, 14, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 14
        { "15_06_2025", new DateTime(2025, 6, 15, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 15
        { "16_06_2025", new DateTime(2025, 6, 16, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 16
        { "17_06_2025", new DateTime(2025, 6, 17, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 17
        { "18_06_2025", new DateTime(2025, 6, 18, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 18
        { "19_06_2025", new DateTime(2025, 6, 19, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 19
        { "20_06_2025", new DateTime(2025, 6, 20, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 20
        { "21_06_2025", new DateTime(2025, 6, 21, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 21
        { "22_06_2025", new DateTime(2025, 6, 22, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 22
        { "23_06_2025", new DateTime(2025, 6, 23, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 23
        { "24_06_2025", new DateTime(2025, 6, 24, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 24
        { "25_06_2025", new DateTime(2025, 6, 25, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 25
        { "26_06_2025", new DateTime(2025, 6, 26, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 26
        { "27_06_2025", new DateTime(2025, 6, 27, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 27
        { "28_06_2025", new DateTime(2025, 6, 28, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 28
        { "29_06_2025", new DateTime(2025, 6, 29, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 29
        { "30_06_2025", new DateTime(2025, 6, 30, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 30
        { "01_07_2025", new DateTime(2025, 7, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 1
        { "02_07_2025", new DateTime(2025, 7, 2, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 2
        { "03_07_2025", new DateTime(2025, 7, 3, 0, 0, 0, DateTimeKind.Utc).AddHours(7) }, // 3
    };
}
