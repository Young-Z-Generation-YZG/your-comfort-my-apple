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
                "690e034dff79797b05b3bc89", // HCM_TD_KVC_1060
                "690e034dff79797b05b3bc12", // HCM_Q1_CMT8_92
                "690e034dff79797b05b3bc13", // HCM_Q9_LVV_33
            };

    public static IEnumerable<string> BranchIds => new List<string>
            {
                "664357a235e84033bbd0e6b6", // Ware house
                "690e034dff79797b05b3bc88", // HCM_TD_KVC_1060
                "690e034dff79797b05b3bc12", // HCM_Q1_CMT8_92
                "690e034dff79797b05b3bc13", // HCM_Q9_LVV_33
            };

    public static IEnumerable<Order> Orders
    {
        get
        {
            var listUserIds = UserIds.ToList();
            var listShippingAddresses = ShippingAddresses.ToList();
            var listTenantId = TenantIds.ToList();
            var listBranchIds = BranchIds.ToList();
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
                                                             List<string> colorOptions,
                                                             List<string> storageOptions,
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
                                var key = $"{model}-{color}-{storage}";
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
            iphone15displayImageUrls = BuildDisplayImageUrls(iphone15ModelNames, iphone15Colors, iphone15Storages, iphone15ColorImageUrls);

            var iphone16displayImageUrls = BuildDisplayImageUrls(iPhone16ModelsList, StoragesList, iPhone16ColorsList, iphone16ColorImageUrls);
            var iphone16EDisplayImageUrls = BuildDisplayImageUrls(iPhone16EModelsList, StoragesList, iPhone16EColorsList, iphone16EColorImageUrls);
            var iphone17displayImageUrls = BuildDisplayImageUrls(iPhone17ModelsList, StoragesList, iPhone17ColorsList, iphone17ColorImageUrls);
            var iphone17ProDisplayImageUrls = BuildDisplayImageUrls(iPhone17ProModelsList, StoragesList, iPhone17ProColorsList, iphone17ProColorImageUrls);
            var iphone17AirDisplayImageUrls = BuildDisplayImageUrls(iPhone17AirModelsList, StoragesList, iPhone17AirColorsList, iphone17AirColorImageUrls);

            string GetDisplayImageUrl(string model, string color, string storage)
            {
                var key = $"{model}-{color}-{storage}";

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
                                 branchId: BranchId.Of(listBranchIds[0]),
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["1_2025"]),
            };

            List<OrderItem> orderItemsList2 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-BLUE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["2_2025"]),
            };

            List<OrderItem> orderItemsList3 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-BLUE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["3_2025"]),
            };

            List<OrderItem> orderItemsList4 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-BLUE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["4_2025"]),
            };

            List<OrderItem> orderItemsList5 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-PINK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["5_2025"]),
            };

            List<OrderItem> orderItemsList6 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-PINK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["6_2025"]),
            };

            List<OrderItem> orderItemsList7 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-PINK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["7_2025"]),
            };

            List<OrderItem> orderItemsList8 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-PINK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["8_2025"]),
            };

            List<OrderItem> orderItemsList9 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-YELLOW-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["9_2025"]),
            };

            List<OrderItem> orderItemsList10 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-YELLOW-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["10_2025"]),
            };

            List<OrderItem> orderItemsList11 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-YELLOW-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["11_2025"]),
            };

            List<OrderItem> orderItemsList12 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-YELLOW-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["12_2025"]),
            };

            List<OrderItem> orderItemsList13 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-GREEN-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["13_2025"]),
            };

            List<OrderItem> orderItemsList14 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-GREEN-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["14_2025"]),
            };

            List<OrderItem> orderItemsList15 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-GREEN-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["15_2025"]),
            };

            List<OrderItem> orderItemsList16 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-GREEN-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["16_2025"]),
            };

            List<OrderItem> orderItemsList17 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-BLACK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["17_2025"]),
            };

            List<OrderItem> orderItemsList18 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-BLACK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["18_2025"]),
            };

            List<OrderItem> orderItemsList19 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-BLACK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["19_2025"]),
            };

            List<OrderItem> orderItemsList20 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15-BLACK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["20_2025"]),
            };

            List<OrderItem> orderItemsList21 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLUE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["21_2025"]),
            };

            List<OrderItem> orderItemsList22 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLUE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["22_2025"]),
            };

            List<OrderItem> orderItemsList23 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLUE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["23_2025"]),
            };

            List<OrderItem> orderItemsList24 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLUE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["24_2025"]),
            };

            List<OrderItem> orderItemsList25 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-PINK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["25_2025"]),
            };

            List<OrderItem> orderItemsList26 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-PINK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["26_2025"]),
            };

            List<OrderItem> orderItemsList27 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-PINK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["27_2025"]),
            };

            List<OrderItem> orderItemsList28 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-PINK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["28_2025"]),
            };

            List<OrderItem> orderItemsList29 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-YELLOW-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["29_2025"]),
            };

            List<OrderItem> orderItemsList30 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-YELLOW-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["30_2025"]),
            };

            List<OrderItem> orderItemsList31 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-YELLOW-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["31_2025"]),
            };

            List<OrderItem> orderItemsList32 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-YELLOW-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["32_2025"]),
            };

            List<OrderItem> orderItemsList33 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-GREEN-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["33_2025"]),
            };

            List<OrderItem> orderItemsList34 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-GREEN-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["34_2025"]),
            };

            List<OrderItem> orderItemsList35 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-GREEN-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["35_2025"]),
            };

            List<OrderItem> orderItemsList36 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-GREEN-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["36_2025"]),
            };

            List<OrderItem> orderItemsList37 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLACK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["37_2025"]),
            };

            List<OrderItem> orderItemsList38 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLACK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["38_2025"]),
            };

            List<OrderItem> orderItemsList39 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLACK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["39_2025"]),
            };

            List<OrderItem> orderItemsList40 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_15_PLUS-BLACK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["40_2025"]),
            };


            // 40 variant items of iPhone 16
            List<OrderItem> orderItemsList41 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-ULTRAMARINE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["41_2025"]),
            };

            List<OrderItem> orderItemsList42 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-ULTRAMARINE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["42_2025"]),
            };

            List<OrderItem> orderItemsList43 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-ULTRAMARINE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["43_2025"]),
            };

            List<OrderItem> orderItemsList44 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-ULTRAMARINE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["44_2025"]),
            };

            List<OrderItem> orderItemsList45 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-TEAL-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["45_2025"]),
            };

            List<OrderItem> orderItemsList46 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-TEAL-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["46_2025"]),
            };

            List<OrderItem> orderItemsList47 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-TEAL-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["47_2025"]),
            };

            List<OrderItem> orderItemsList48 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-TEAL-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["48_2025"]),
            };

            List<OrderItem> orderItemsList49 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-PINK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["49_2025"]),
            };

            List<OrderItem> orderItemsList50 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-PINK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["50_2025"]),
            };

            List<OrderItem> orderItemsList51 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-PINK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["51_2025"]),
            };

            List<OrderItem> orderItemsList52 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-PINK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["52_2025"]),
            };

            List<OrderItem> orderItemsList53 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-WHITE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["53_2025"]),
            };

            List<OrderItem> orderItemsList54 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-WHITE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["54_2025"]),
            };

            List<OrderItem> orderItemsList55 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-WHITE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["55_2025"]),
            };

            List<OrderItem> orderItemsList56 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-WHITE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["56_2025"]),
            };

            List<OrderItem> orderItemsList57 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-BLACK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["57_2025"]),
            };

            List<OrderItem> orderItemsList58 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-BLACK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["58_2025"]),
            };

            List<OrderItem> orderItemsList59 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-BLACK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["59_2025"]),
            };

            List<OrderItem> orderItemsList60 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16-BLACK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["60_2025"]),
            };

            List<OrderItem> orderItemsList61 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-ULTRAMARINE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["61_2025"]),
            };

            List<OrderItem> orderItemsList62 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-ULTRAMARINE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["62_2025"]),
            };

            List<OrderItem> orderItemsList63 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-ULTRAMARINE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["63_2025"]),
            };

            List<OrderItem> orderItemsList64 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-ULTRAMARINE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["64_2025"]),
            };

            List<OrderItem> orderItemsList65 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-TEAL-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["65_2025"]),
            };

            List<OrderItem> orderItemsList66 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-TEAL-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["66_2025"]),
            };

            List<OrderItem> orderItemsList67 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-TEAL-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["67_2025"]),
            };

            List<OrderItem> orderItemsList68 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-TEAL-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["68_2025"]),
            };

            List<OrderItem> orderItemsList69 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-PINK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["69_2025"]),
            };

            List<OrderItem> orderItemsList70 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-PINK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["70_2025"]),
            };

            List<OrderItem> orderItemsList71 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-PINK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["71_2025"]),
            };

            List<OrderItem> orderItemsList72 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-PINK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["72_2025"]),
            };

            List<OrderItem> orderItemsList73 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-WHITE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["73_2025"]),
            };

            List<OrderItem> orderItemsList74 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-WHITE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["74_2025"]),
            };

            List<OrderItem> orderItemsList75 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-WHITE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["75_2025"]),
            };

            List<OrderItem> orderItemsList76 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-WHITE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["76_2025"]),
            };

            List<OrderItem> orderItemsList77 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-BLACK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["77_2025"]),
            };

            List<OrderItem> orderItemsList78 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-BLACK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["78_2025"]),
            };

            List<OrderItem> orderItemsList79 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-BLACK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["79_2025"]),
            };

            List<OrderItem> orderItemsList80 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16_PLUS-BLACK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["80_2025"]),
            };


            // 8 variant items of iPhone 16e
            List<OrderItem> orderItemsList81 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-WHITE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["81_2025"]),
            };

            List<OrderItem> orderItemsList82 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-WHITE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["82_2025"]),
            };

            List<OrderItem> orderItemsList83 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-WHITE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["83_2025"]),
            };

            List<OrderItem> orderItemsList84 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-WHITE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["84_2025"]),
            };

            List<OrderItem> orderItemsList85 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-BLACK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["85_2025"]),
            };

            List<OrderItem> orderItemsList86 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-BLACK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["86_2025"]),
            };

            List<OrderItem> orderItemsList87 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-BLACK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["87_2025"]),
            };

            List<OrderItem> orderItemsList88 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_16E-BLACK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["88_2025"]),
            };


            // 20 variant items of iPhone 17
            List<OrderItem> orderItemsList89 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-LAVENDER-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["89_2025"]),
            };

            List<OrderItem> orderItemsList90 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-LAVENDER-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["90_2025"]),
            };

            List<OrderItem> orderItemsList91 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-LAVENDER-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["91_2025"]),
            };

            List<OrderItem> orderItemsList92 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-LAVENDER-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["92_2025"]),
            };

            List<OrderItem> orderItemsList93 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-SAGE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["93_2025"]),
            };

            List<OrderItem> orderItemsList94 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-SAGE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["94_2025"]),
            };

            List<OrderItem> orderItemsList95 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-SAGE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["95_2025"]),
            };

            List<OrderItem> orderItemsList96 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-SAGE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["96_2025"]),
            };

            List<OrderItem> orderItemsList97 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-MIST_BLUE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["97_2025"]),
            };

            List<OrderItem> orderItemsList98 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-MIST_BLUE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["98_2025"]),
            };

            List<OrderItem> orderItemsList99 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-MIST_BLUE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["99_2025"]),
            };

            List<OrderItem> orderItemsList100 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-MIST_BLUE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["100_2025"]),
            };

            List<OrderItem> orderItemsList101 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-WHITE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["101_2025"]),
            };

            List<OrderItem> orderItemsList102 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-WHITE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["102_2025"]),
            };

            List<OrderItem> orderItemsList103 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-WHITE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["103_2025"]),
            };

            List<OrderItem> orderItemsList104 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-WHITE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["104_2025"]),
            };

            List<OrderItem> orderItemsList105 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-BLACK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["105_2025"]),
            };

            List<OrderItem> orderItemsList106 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-BLACK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["106_2025"]),
            };

            List<OrderItem> orderItemsList107 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-BLACK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["107_2025"]),
            };

            List<OrderItem> orderItemsList108 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17-BLACK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["108_2025"]),
            };



            // 24 variant items of iPhone 17 Pro
            List<OrderItem> orderItemsList109 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-SILVER-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["109_2025"]),
            };

            List<OrderItem> orderItemsList110 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-SILVER-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["110_2025"]),
            };

            List<OrderItem> orderItemsList111 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-SILVER-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["111_2025"]),
            };

            List<OrderItem> orderItemsList112 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-SILVER-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["112_2025"]),
            };

            List<OrderItem> orderItemsList113 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-COSMIC_ORANGE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["113_2025"]),
            };

            List<OrderItem> orderItemsList114 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-COSMIC_ORANGE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["114_2025"]),
            };

            List<OrderItem> orderItemsList115 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-COSMIC_ORANGE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["115_2025"]),
            };

            List<OrderItem> orderItemsList116 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-COSMIC_ORANGE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["116_2025"]),
            };

            List<OrderItem> orderItemsList117 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-DEEP_BLUE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["117_2025"]),
            };

            List<OrderItem> orderItemsList118 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-DEEP_BLUE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["118_2025"]),
            };

            List<OrderItem> orderItemsList119 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-DEEP_BLUE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["119_2025"]),
            };

            List<OrderItem> orderItemsList120 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO-DEEP_BLUE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["120_2025"]),
            };

            List<OrderItem> orderItemsList121 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-SILVER-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["121_2025"]),
            };

            List<OrderItem> orderItemsList122 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-SILVER-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["122_2025"]),
            };

            List<OrderItem> orderItemsList123 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-SILVER-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["123_2025"]),
            };

            List<OrderItem> orderItemsList124 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-SILVER-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["124_2025"]),
            };

            List<OrderItem> orderItemsList125 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-COSMIC_ORANGE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["125_2025"]),
            };

            List<OrderItem> orderItemsList126 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-COSMIC_ORANGE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["126_2025"]),
            };

            List<OrderItem> orderItemsList127 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-COSMIC_ORANGE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["127_2025"]),
            };

            List<OrderItem> orderItemsList128 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-COSMIC_ORANGE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["128_2025"]),
            };

            List<OrderItem> orderItemsList129 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-DEEP_BLUE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["129_2025"]),
            };

            List<OrderItem> orderItemsList130 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-DEEP_BLUE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["130_2025"]),
            };

            List<OrderItem> orderItemsList131 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-DEEP_BLUE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["131_2025"]),
            };

            List<OrderItem> orderItemsList132 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_PRO_MAX-DEEP_BLUE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["132_2025"]),
            };


            // 16 variant items of iPhone 17 Air
            List<OrderItem> orderItemsList133 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SKY_BLUE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["133_2025"]),
            };

            List<OrderItem> orderItemsList134 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SKY_BLUE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["134_2025"]),
            };

            List<OrderItem> orderItemsList135 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SKY_BLUE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["135_2025"]),
            };

            List<OrderItem> orderItemsList136 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SKY_BLUE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["136_2025"]),
            };

            List<OrderItem> orderItemsList137 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-LIGHT_GOLD-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["137_2025"]),
            };

            List<OrderItem> orderItemsList138 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-LIGHT_GOLD-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["138_2025"]),
            };

            List<OrderItem> orderItemsList139 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-LIGHT_GOLD-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["139_2025"]),
            };

            List<OrderItem> orderItemsList140 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-LIGHT_GOLD-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["140_2025"]),
            };

            List<OrderItem> orderItemsList141 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-CLOUD_WHITE-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["141_2025"]),
            };

            List<OrderItem> orderItemsList142 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-CLOUD_WHITE-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["142_2025"]),
            };

            List<OrderItem> orderItemsList143 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-CLOUD_WHITE-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["143_2025"]),
            };

            List<OrderItem> orderItemsList144 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-CLOUD_WHITE-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["144_2025"]),
            };

            List<OrderItem> orderItemsList145 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SPACE_BLACK-128GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["145_2025"]),
            };

            List<OrderItem> orderItemsList146 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SPACE_BLACK-256GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["146_2025"]),
            };

            List<OrderItem> orderItemsList147 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SPACE_BLACK-512GB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["147_2025"]),
            };

            List<OrderItem> orderItemsList148 = new List<OrderItem>
            {
                OrderItem.Create(orderItemId: OrderItemId.Create(), // IPHONE-IPHONE_17_AIR-SPACE_BLACK-1TB
                                 tenantId: TenantId.Of(listTenantId[0]),
                                 branchId: BranchId.Of(listBranchIds[0]),
                                 orderId: OrderId.Create(),
                                 skuId: "SEED_DATA",
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
                                 isReviewed: false,
                                 createdAt: CreatedAtDateTimes["148_2025"]),
            };


















            // 40 orders of iPhone 15
            var order1 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: BranchId.Of(listBranchIds[0]),
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
                                      createdAt: CreatedAtDateTimes["1_2025"]);
            order1.AddOrderItem(orderItemsList1[0]);

            var order2 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: BranchId.Of(listBranchIds[0]),
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
                                      createdAt: CreatedAtDateTimes["2_2025"]);
            order2.AddOrderItem(orderItemsList2[0]);

            var order3 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: BranchId.Of(listBranchIds[0]),
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
                                      createdAt: CreatedAtDateTimes["3_2025"]);
            order3.AddOrderItem(orderItemsList3[0]);

            var order4 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: BranchId.Of(listBranchIds[0]),
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
                                      createdAt: CreatedAtDateTimes["4_2025"]);
            order4.AddOrderItem(orderItemsList4[0]);

            var order5 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: BranchId.Of(listBranchIds[0]),
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
                                      createdAt: CreatedAtDateTimes["5_2025"]);
            order5.AddOrderItem(orderItemsList5[0]);

            var order6 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: BranchId.Of(listBranchIds[0]),
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
                                      createdAt: CreatedAtDateTimes["6_2025"]);
            order6.AddOrderItem(orderItemsList6[0]);

            var order7 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: BranchId.Of(listBranchIds[0]),
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
                                      createdAt: CreatedAtDateTimes["7_2025"]);
            order7.AddOrderItem(orderItemsList7[0]);

            var order8 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: BranchId.Of(listBranchIds[0]),
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
                                      createdAt: CreatedAtDateTimes["8_2025"]);
            order8.AddOrderItem(orderItemsList8[0]);

            var order9 = Order.Create(orderId: OrderId.Create(),
                                      tenantId: TenantId.Of(listTenantId[0]),
                                      branchId: BranchId.Of(listBranchIds[0]),
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
                                      createdAt: CreatedAtDateTimes["9_2025"]);
            order9.AddOrderItem(orderItemsList9[0]);

            var order10 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["10_2025"]);
            order10.AddOrderItem(orderItemsList10[0]);

            var order11 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["11_2025"]);
            order11.AddOrderItem(orderItemsList11[0]);

            var order12 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["12_2025"]);
            order12.AddOrderItem(orderItemsList12[0]);

            var order13 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["13_2025"]);
            order13.AddOrderItem(orderItemsList13[0]);

            var order14 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["14_2025"]);
            order14.AddOrderItem(orderItemsList14[0]);

            var order15 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["15_2025"]);
            order15.AddOrderItem(orderItemsList15[0]);

            var order16 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["16_2025"]);
            order16.AddOrderItem(orderItemsList16[0]);

            var order17 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["17_2025"]);
            order17.AddOrderItem(orderItemsList17[0]);

            var order18 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["18_2025"]);
            order18.AddOrderItem(orderItemsList18[0]);

            var order19 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["19_2025"]);
            order19.AddOrderItem(orderItemsList19[0]);

            var order20 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["20_2025"]);
            order20.AddOrderItem(orderItemsList20[0]);

            var order21 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["21_2025"]);
            order21.AddOrderItem(orderItemsList21[0]);

            var order22 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["22_2025"]);
            order22.AddOrderItem(orderItemsList22[0]);

            var order23 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["23_2025"]);
            order23.AddOrderItem(orderItemsList23[0]);

            var order24 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["24_2025"]);
            order24.AddOrderItem(orderItemsList24[0]);

            var order25 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["25_2025"]);
            order25.AddOrderItem(orderItemsList25[0]);

            var order26 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["26_2025"]);
            order26.AddOrderItem(orderItemsList26[0]);

            var order27 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["27_2025"]);
            order27.AddOrderItem(orderItemsList27[0]);

            var order28 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["28_2025"]);
            order28.AddOrderItem(orderItemsList28[0]);

            var order29 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["29_2025"]);
            order29.AddOrderItem(orderItemsList29[0]);

            var order30 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["30_2025"]);
            order30.AddOrderItem(orderItemsList30[0]);

            var order31 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["31_2025"]);
            order31.AddOrderItem(orderItemsList31[0]);

            var order32 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["32_2025"]);
            order32.AddOrderItem(orderItemsList32[0]);

            var order33 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["33_2025"]);
            order33.AddOrderItem(orderItemsList33[0]);

            var order34 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["34_2025"]);
            order34.AddOrderItem(orderItemsList34[0]);

            var order35 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["35_2025"]);
            order35.AddOrderItem(orderItemsList35[0]);

            var order36 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["36_2025"]);
            order36.AddOrderItem(orderItemsList36[0]);

            var order37 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["37_2025"]);
            order37.AddOrderItem(orderItemsList37[0]);

            var order38 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["38_2025"]);
            order38.AddOrderItem(orderItemsList38[0]);

            var order39 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["39_2025"]);
            order39.AddOrderItem(orderItemsList39[0]);

            var order40 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["40_2025"]);
            order40.AddOrderItem(orderItemsList40[0]);




            // 40 orders of iPhone 16
            var order41 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["41_2025"]);
            order41.AddOrderItem(orderItemsList41[0]);

            var order42 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["42_2025"]);
            order42.AddOrderItem(orderItemsList42[0]);

            var order43 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["43_2025"]);
            order43.AddOrderItem(orderItemsList43[0]);

            var order44 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["44_2025"]);
            order44.AddOrderItem(orderItemsList44[0]);

            var order45 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["45_2025"]);
            order45.AddOrderItem(orderItemsList45[0]);

            var order46 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["46_2025"]);
            order46.AddOrderItem(orderItemsList46[0]);

            var order47 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["47_2025"]);
            order47.AddOrderItem(orderItemsList47[0]);

            var order48 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["48_2025"]);
            order48.AddOrderItem(orderItemsList48[0]);

            var order49 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["49_2025"]);
            order49.AddOrderItem(orderItemsList49[0]);

            var order50 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["50_2025"]);
            order50.AddOrderItem(orderItemsList50[0]);

            var order51 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["51_2025"]);
            order51.AddOrderItem(orderItemsList51[0]);

            var order52 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["52_2025"]);
            order52.AddOrderItem(orderItemsList52[0]);

            var order53 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["53_2025"]);
            order53.AddOrderItem(orderItemsList53[0]);

            var order54 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["54_2025"]);
            order54.AddOrderItem(orderItemsList54[0]);

            var order55 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["55_2025"]);
            order55.AddOrderItem(orderItemsList55[0]);

            var order56 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["56_2025"]);
            order56.AddOrderItem(orderItemsList56[0]);

            var order57 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["57_2025"]);
            order57.AddOrderItem(orderItemsList57[0]);

            var order58 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["58_2025"]);
            order58.AddOrderItem(orderItemsList58[0]);

            var order59 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["59_2025"]);
            order59.AddOrderItem(orderItemsList59[0]);

            var order60 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["60_2025"]);
            order60.AddOrderItem(orderItemsList60[0]);

            var order61 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["61_2025"]);
            order61.AddOrderItem(orderItemsList61[0]);

            var order62 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["62_2025"]);
            order62.AddOrderItem(orderItemsList62[0]);

            var order63 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["63_2025"]);
            order63.AddOrderItem(orderItemsList63[0]);

            var order64 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["64_2025"]);
            order64.AddOrderItem(orderItemsList64[0]);

            var order65 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["65_2025"]);
            order65.AddOrderItem(orderItemsList65[0]);

            var order66 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["66_2025"]);
            order66.AddOrderItem(orderItemsList66[0]);

            var order67 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["67_2025"]);
            order67.AddOrderItem(orderItemsList67[0]);

            var order68 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["68_2025"]);
            order68.AddOrderItem(orderItemsList68[0]);

            var order69 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["69_2025"]);
            order69.AddOrderItem(orderItemsList69[0]);

            var order70 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["70_2025"]);
            order70.AddOrderItem(orderItemsList70[0]);

            var order71 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["71_2025"]);
            order71.AddOrderItem(orderItemsList71[0]);

            var order72 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["72_2025"]);
            order72.AddOrderItem(orderItemsList72[0]);

            var order73 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["73_2025"]);
            order73.AddOrderItem(orderItemsList73[0]);

            var order74 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["74_2025"]);
            order74.AddOrderItem(orderItemsList74[0]);

            var order75 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["75_2025"]);
            order75.AddOrderItem(orderItemsList75[0]);

            var order76 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["76_2025"]);
            order76.AddOrderItem(orderItemsList76[0]);

            var order77 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["77_2025"]);
            order77.AddOrderItem(orderItemsList77[0]);

            var order78 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["78_2025"]);
            order78.AddOrderItem(orderItemsList78[0]);

            var order79 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["79_2025"]);
            order79.AddOrderItem(orderItemsList79[0]);

            var order80 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["80_2025"]);
            order80.AddOrderItem(orderItemsList80[0]);


            // 8 orders of iPhone 16e
            var order81 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["81_2025"]);
            order81.AddOrderItem(orderItemsList81[0]);

            var order82 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["82_2025"]);
            order82.AddOrderItem(orderItemsList82[0]);

            var order83 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["83_2025"]);
            order83.AddOrderItem(orderItemsList83[0]);

            var order84 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["84_2025"]);
            order84.AddOrderItem(orderItemsList84[0]);

            var order85 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["85_2025"]);
            order85.AddOrderItem(orderItemsList85[0]);

            var order86 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["86_2025"]);
            order86.AddOrderItem(orderItemsList86[0]);

            var order87 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["87_2025"]);
            order87.AddOrderItem(orderItemsList87[0]);

            var order88 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["88_2025"]);
            order88.AddOrderItem(orderItemsList88[0]);


            // 20 orders of iPhone 17
            var order89 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["89_2025"]);
            order89.AddOrderItem(orderItemsList89[0]);

            var order90 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["90_2025"]);
            order90.AddOrderItem(orderItemsList90[0]);

            var order91 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["91_2025"]);
            order91.AddOrderItem(orderItemsList91[0]);

            var order92 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["92_2025"]);
            order92.AddOrderItem(orderItemsList92[0]);

            var order93 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["93_2025"]);
            order93.AddOrderItem(orderItemsList93[0]);

            var order94 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["94_2025"]);
            order94.AddOrderItem(orderItemsList94[0]);

            var order95 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["95_2025"]);
            order95.AddOrderItem(orderItemsList95[0]);

            var order96 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["96_2025"]);
            order96.AddOrderItem(orderItemsList96[0]);

            var order97 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["97_2025"]);
            order97.AddOrderItem(orderItemsList97[0]);

            var order98 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["98_2025"]);
            order98.AddOrderItem(orderItemsList98[0]);

            var order99 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["99_2025"]);
            order99.AddOrderItem(orderItemsList99[0]);

            var order100 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["100_2025"]);
            order100.AddOrderItem(orderItemsList100[0]);

            var order101 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["101_2025"]);
            order101.AddOrderItem(orderItemsList101[0]);

            var order102 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["102_2025"]);
            order102.AddOrderItem(orderItemsList102[0]);

            var order103 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["103_2025"]);
            order103.AddOrderItem(orderItemsList103[0]);

            var order104 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["104_2025"]);
            order104.AddOrderItem(orderItemsList104[0]);

            var order105 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["105_2025"]);
            order105.AddOrderItem(orderItemsList105[0]);

            var order106 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["106_2025"]);
            order106.AddOrderItem(orderItemsList106[0]);

            var order107 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["107_2025"]);
            order107.AddOrderItem(orderItemsList107[0]);

            var order108 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["108_2025"]);
            order108.AddOrderItem(orderItemsList108[0]);


            // 24 orders of iPhone 17 Pro
            var order109 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["109_2025"]);
            order109.AddOrderItem(orderItemsList109[0]);

            var order110 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["110_2025"]);
            order110.AddOrderItem(orderItemsList110[0]);

            var order111 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["111_2025"]);
            order111.AddOrderItem(orderItemsList111[0]);

            var order112 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["112_2025"]);
            order112.AddOrderItem(orderItemsList112[0]);

            var order113 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["113_2025"]);
            order113.AddOrderItem(orderItemsList113[0]);

            var order114 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["114_2025"]);
            order114.AddOrderItem(orderItemsList114[0]);

            var order115 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["115_2025"]);
            order115.AddOrderItem(orderItemsList115[0]);

            var order116 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["116_2025"]);
            order116.AddOrderItem(orderItemsList116[0]);

            var order117 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["117_2025"]);
            order117.AddOrderItem(orderItemsList117[0]);

            var order118 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["118_2025"]);
            order118.AddOrderItem(orderItemsList118[0]);

            var order119 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["119_2025"]);
            order119.AddOrderItem(orderItemsList119[0]);

            var order120 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["120_2025"]);
            order120.AddOrderItem(orderItemsList120[0]);

            var order121 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["121_2025"]);
            order121.AddOrderItem(orderItemsList121[0]);

            var order122 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["122_2025"]);
            order122.AddOrderItem(orderItemsList122[0]);

            var order123 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["123_2025"]);
            order123.AddOrderItem(orderItemsList123[0]);

            var order124 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["124_2025"]);
            order124.AddOrderItem(orderItemsList124[0]);

            var order125 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["125_2025"]);
            order125.AddOrderItem(orderItemsList125[0]);

            var order126 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["126_2025"]);
            order126.AddOrderItem(orderItemsList126[0]);

            var order127 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["127_2025"]);
            order127.AddOrderItem(orderItemsList127[0]);

            var order128 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["128_2025"]);
            order128.AddOrderItem(orderItemsList128[0]);

            var order129 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["129_2025"]);
            order129.AddOrderItem(orderItemsList129[0]);

            var order130 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["130_2025"]);
            order130.AddOrderItem(orderItemsList130[0]);

            var order131 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["131_2025"]);
            order131.AddOrderItem(orderItemsList131[0]);

            var order132 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["132_2025"]);
            order132.AddOrderItem(orderItemsList132[0]);


            // 16 orders of iPhone 17 Air
            var order133 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["133_2025"]);
            order133.AddOrderItem(orderItemsList133[0]);

            var order134 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["134_2025"]);
            order134.AddOrderItem(orderItemsList134[0]);

            var order135 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["135_2025"]);
            order135.AddOrderItem(orderItemsList135[0]);

            var order136 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["136_2025"]);
            order136.AddOrderItem(orderItemsList136[0]);

            var order137 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["137_2025"]);
            order137.AddOrderItem(orderItemsList137[0]);

            var order138 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["138_2025"]);
            order138.AddOrderItem(orderItemsList138[0]);

            var order139 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["139_2025"]);
            order139.AddOrderItem(orderItemsList139[0]);

            var order140 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["140_2025"]);
            order140.AddOrderItem(orderItemsList140[0]);

            var order141 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["141_2025"]);
            order141.AddOrderItem(orderItemsList141[0]);

            var order142 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["142_2025"]);
            order142.AddOrderItem(orderItemsList142[0]);

            var order143 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["143_2025"]);
            order143.AddOrderItem(orderItemsList143[0]);

            var order144 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["144_2025"]);
            order144.AddOrderItem(orderItemsList144[0]);

            var order145 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["145_2025"]);
            order145.AddOrderItem(orderItemsList145[0]);

            var order146 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["146_2025"]);
            order146.AddOrderItem(orderItemsList146[0]);

            var order147 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["147_2025"]);
            order147.AddOrderItem(orderItemsList147[0]);

            var order148 = Order.Create(orderId: OrderId.Create(),
                                       tenantId: TenantId.Of(listTenantId[0]),
                                       branchId: BranchId.Of(listBranchIds[0]),
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
                                       createdAt: CreatedAtDateTimes["148_2025"]);
            order148.AddOrderItem(orderItemsList148[0]);

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

    // Helper method to create natural timestamps with time variations
    private static DateTime CreateNaturalDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc).AddHours(7);
    }

    public static Dictionary<string, DateTime> CreatedAtDateTimes => new Dictionary<string, DateTime>
    {
        // Jany 2025 - Natural distribution with time variations
        { "1_2025", CreateNaturalDateTime(2025, 1, 3, 14, 23, 45) }, // Jan 3 - Friday afternoon
        { "2_2025", CreateNaturalDateTime(2025, 1, 4, 10, 15, 30) }, // Jan 4 - Saturday morning
        { "3_2025", CreateNaturalDateTime(2025, 1, 6, 16, 42, 18) }, // Jan 6 - Monday afternoon
        { "4_2025", CreateNaturalDateTime(2025, 1, 7, 11, 8, 52) }, // Jan 7 - Tuesday morning
        { "5_2025", CreateNaturalDateTime(2025, 1, 8, 15, 33, 27) }, // Jan 8 - Wednesday afternoon
        { "6_2025", CreateNaturalDateTime(2025, 1, 9, 9, 47, 13) }, // Jan 9 - Thursday morning
        { "7_2025", CreateNaturalDateTime(2025, 1, 9, 18, 22, 8) }, // Jan 9 - Thursday evening (2nd order)
        { "8_2025", CreateNaturalDateTime(2025, 1, 10, 13, 55, 41) }, // Jan 10 - Friday afternoon
        { "9_2025", CreateNaturalDateTime(2025, 1, 13, 10, 28, 19) }, // Jan 13 - Monday morning
        { "10_2025", CreateNaturalDateTime(2025, 1, 14, 14, 12, 36) }, // Jan 14 - Tuesday afternoon
        { "11_2025", CreateNaturalDateTime(2025, 1, 15, 11, 39, 54) }, // Jan 15 - Wednesday morning
        { "12_2025", CreateNaturalDateTime(2025, 1, 16, 16, 7, 22) }, // Jan 16 - Thursday afternoon
        { "13_2025", CreateNaturalDateTime(2025, 1, 16, 19, 45, 33) }, // Jan 16 - Thursday evening (2nd order)
        { "14_2025", CreateNaturalDateTime(2025, 1, 17, 9, 18, 7) }, // Jan 17 - Friday morning
        { "15_2025", CreateNaturalDateTime(2025, 1, 20, 15, 26, 48) }, // Jan 20 - Monday afternoon
        { "16_2025", CreateNaturalDateTime(2025, 1, 21, 12, 4, 15) }, // Jan 21 - Tuesday afternoon
        { "17_2025", CreateNaturalDateTime(2025, 1, 22, 10, 51, 29) }, // Jan 22 - Wednesday morning
        { "18_2025", CreateNaturalDateTime(2025, 1, 23, 17, 14, 56) }, // Jan 23 - Thursday afternoon
        { "19_2025", CreateNaturalDateTime(2025, 1, 24, 11, 33, 42) }, // Jan 24 - Friday morning
        { "20_2025", CreateNaturalDateTime(2025, 1, 27, 14, 8, 25) }, // Jan 27 - Monday afternoon
        { "21_2025", CreateNaturalDateTime(2025, 1, 28, 9, 57, 11) }, // Jan 28 - Tuesday morning
        { "22_2025", CreateNaturalDateTime(2025, 1, 29, 16, 21, 38) }, // Jan 29 - Wednesday afternoon
        { "23_2025", CreateNaturalDateTime(2025, 1, 30, 13, 46, 9) }, // Jan 30 - Thursday afternoon
        { "24_2025", CreateNaturalDateTime(2025, 1, 30, 20, 12, 44) }, // Jan 30 - Thursday evening (2nd order)
        { "25_2025", CreateNaturalDateTime(2025, 1, 31, 10, 35, 17) }, // Jan 31 - Friday morning
        
        // Febry 2025 - Natural distribution
        { "26_2025", CreateNaturalDateTime(2025, 2, 3, 15, 19, 52) }, // Feb 3 - Monday afternoon
        { "27_2025", CreateNaturalDateTime(2025, 2, 4, 11, 42, 28) }, // Feb 4 - Tuesday morning
        { "28_2025", CreateNaturalDateTime(2025, 2, 5, 14, 55, 6) }, // Feb 5 - Wednesday afternoon
        { "29_2025", CreateNaturalDateTime(2025, 2, 6, 9, 23, 41) }, // Feb 6 - Thursday morning
        { "30_2025", CreateNaturalDateTime(2025, 2, 7, 17, 8, 33) }, // Feb 7 - Friday afternoon
        { "31_2025", CreateNaturalDateTime(2025, 2, 10, 12, 16, 19) }, // Feb 10 - Monday afternoon
        { "32_2025", CreateNaturalDateTime(2025, 2, 11, 10, 49, 57) }, // Feb 11 - Tuesday morning
        { "33_2025", CreateNaturalDateTime(2025, 2, 11, 18, 32, 24) }, // Feb 11 - Tuesday evening (2nd order)
        { "34_2025", CreateNaturalDateTime(2025, 2, 12, 15, 4, 8) }, // Feb 12 - Wednesday afternoon
        { "35_2025", CreateNaturalDateTime(2025, 2, 13, 11, 27, 45) }, // Feb 13 - Thursday morning
        { "36_2025", CreateNaturalDateTime(2025, 2, 14, 16, 38, 52) }, // Feb 14 - Friday afternoon
        { "37_2025", CreateNaturalDateTime(2025, 2, 17, 9, 14, 26) }, // Feb 17 - Monday morning
        { "38_2025", CreateNaturalDateTime(2025, 2, 18, 14, 51, 39) }, // Feb 18 - Tuesday afternoon
        { "39_2025", CreateNaturalDateTime(2025, 2, 19, 13, 6, 12) }, // Feb 19 - Wednesday afternoon
        { "40_2025", CreateNaturalDateTime(2025, 2, 20, 10, 33, 58) }, // Feb 20 - Thursday morning
        { "41_2025", CreateNaturalDateTime(2025, 2, 21, 17, 25, 14) }, // Feb 21 - Friday afternoon
        { "42_2025", CreateNaturalDateTime(2025, 2, 24, 11, 48, 7) }, // Feb 24 - Monday morning
        { "43_2025", CreateNaturalDateTime(2025, 2, 25, 15, 12, 43) }, // Feb 25 - Tuesday afternoon
        { "44_2025", CreateNaturalDateTime(2025, 2, 26, 9, 37, 29) }, // Feb 26 - Wednesday morning
        { "45_2025", CreateNaturalDateTime(2025, 2, 27, 16, 44, 51) }, // Feb 27 - Thursday afternoon
        { "46_2025", CreateNaturalDateTime(2025, 2, 28, 12, 19, 35) }, // Feb 28 - Friday afternoon
        
        // Mar2025 - Natural distribution
        { "47_2025", CreateNaturalDateTime(2025, 3, 2, 14, 28, 16) }, // Mar 2 - Sunday afternoon
        { "48_2025", CreateNaturalDateTime(2025, 3, 3, 10, 52, 42) }, // Mar 3 - Monday morning
        { "49_2025", CreateNaturalDateTime(2025, 3, 4, 15, 7, 23) }, // Mar 4 - Tuesday afternoon
        { "50_2025", CreateNaturalDateTime(2025, 3, 5, 11, 31, 8) }, // Mar 5 - Wednesday morning
        { "51_2025", CreateNaturalDateTime(2025, 3, 6, 18, 15, 47) }, // Mar 6 - Thursday evening
        { "52_2025", CreateNaturalDateTime(2025, 3, 7, 9, 44, 19) }, // Mar 7 - Friday morning
        { "53_2025", CreateNaturalDateTime(2025, 3, 10, 13, 26, 54) }, // Mar 10 - Monday afternoon
        { "54_2025", CreateNaturalDateTime(2025, 3, 11, 16, 39, 28) }, // Mar 11 - Tuesday afternoon
        { "55_2025", CreateNaturalDateTime(2025, 3, 12, 10, 17, 41) }, // Mar 12 - Wednesday morning
        { "56_2025", CreateNaturalDateTime(2025, 3, 13, 14, 53, 7) }, // Mar 13 - Thursday afternoon
        { "57_2025", CreateNaturalDateTime(2025, 3, 14, 12, 8, 32) }, // Mar 14 - Friday afternoon
        { "58_2025", CreateNaturalDateTime(2025, 3, 17, 9, 35, 18) }, // Mar 17 - Monday morning
        { "59_2025", CreateNaturalDateTime(2025, 3, 18, 15, 21, 46) }, // Mar 18 - Tuesday afternoon
        { "60_2025", CreateNaturalDateTime(2025, 3, 19, 11, 49, 23) }, // Mar 19 - Wednesday morning
        { "61_2025", CreateNaturalDateTime(2025, 3, 20, 17, 4, 59) }, // Mar 20 - Thursday afternoon
        { "62_2025", CreateNaturalDateTime(2025, 3, 21, 13, 27, 14) }, // Mar 21 - Friday afternoon
        { "63_2025", CreateNaturalDateTime(2025, 3, 24, 10, 41, 37) }, // Mar 24 - Monday morning
        { "64_2025", CreateNaturalDateTime(2025, 3, 25, 16, 18, 52) }, // Mar 25 - Tuesday afternoon
        { "65_2025", CreateNaturalDateTime(2025, 3, 26, 12, 55, 8) }, // Mar 26 - Wednesday afternoon
        { "66_2025", CreateNaturalDateTime(2025, 3, 27, 9, 22, 44) }, // Mar 27 - Thursday morning
        { "67_2025", CreateNaturalDateTime(2025, 3, 28, 14, 36, 21) }, // Mar 28 - Friday afternoon
        { "68_2025", CreateNaturalDateTime(2025, 3, 28, 19, 11, 48) }, // Mar 28 - Friday evening (2nd order)
        { "69_2025", CreateNaturalDateTime(2025, 3, 31, 11, 13, 29) }, // Mar 31 - Monday morning
        
        // Apr2025 - Natural distribution with growth trend
        { "70_2025", CreateNaturalDateTime(2025, 4, 1, 15, 47, 16) }, // Apr 1 - Tuesday afternoon
        { "71_2025", CreateNaturalDateTime(2025, 4, 2, 10, 29, 53) }, // Apr 2 - Wednesday morning
        { "72_2025", CreateNaturalDateTime(2025, 4, 3, 17, 32, 28) }, // Apr 3 - Thursday afternoon
        { "73_2025", CreateNaturalDateTime(2025, 4, 4, 12, 15, 41) }, // Apr 4 - Friday afternoon
        { "74_2025", CreateNaturalDateTime(2025, 4, 7, 9, 38, 27) }, // Apr 7 - Monday morning
        { "75_2025", CreateNaturalDateTime(2025, 4, 8, 14, 52, 19) }, // Apr 8 - Tuesday afternoon
        { "76_2025", CreateNaturalDateTime(2025, 4, 8, 18, 24, 36) }, // Apr 8 - Tuesday evening (2nd order)
        { "77_2025", CreateNaturalDateTime(2025, 4, 9, 11, 6, 48) }, // Apr 9 - Wednesday morning
        { "78_2025", CreateNaturalDateTime(2025, 4, 9, 16, 41, 12) }, // Apr 9 - Wednesday afternoon (3rd order)
        { "79_2025", CreateNaturalDateTime(2025, 4, 10, 13, 28, 55) }, // Apr 10 - Thursday afternoon
        { "80_2025", CreateNaturalDateTime(2025, 4, 11, 10, 52, 33) }, // Apr 11 - Friday morning
        { "81_2025", CreateNaturalDateTime(2025, 4, 14, 15, 19, 47) }, // Apr 14 - Monday afternoon
        { "82_2025", CreateNaturalDateTime(2025, 4, 15, 9, 44, 21) }, // Apr 15 - Tuesday morning
        { "83_2025", CreateNaturalDateTime(2025, 4, 15, 17, 8, 14) }, // Apr 15 - Tuesday evening (2nd order)
        { "84_2025", CreateNaturalDateTime(2025, 4, 16, 12, 35, 58) }, // Apr 16 - Wednesday afternoon
        { "85_2025", CreateNaturalDateTime(2025, 4, 17, 16, 27, 42) }, // Apr 17 - Thursday afternoon
        { "86_2025", CreateNaturalDateTime(2025, 4, 18, 11, 13, 29) }, // Apr 18 - Friday morning
        { "87_2025", CreateNaturalDateTime(2025, 4, 21, 14, 48, 16) }, // Apr 21 - Monday afternoon
        { "88_2025", CreateNaturalDateTime(2025, 4, 22, 10, 31, 7) }, // Apr 22 - Tuesday morning
        { "89_2025", CreateNaturalDateTime(2025, 4, 22, 19, 5, 43) }, // Apr 22 - Tuesday evening (2nd order)
        { "90_2025", CreateNaturalDateTime(2025, 4, 23, 15, 52, 28) }, // Apr 23 - Wednesday afternoon
        { "91_2025", CreateNaturalDateTime(2025, 4, 24, 13, 17, 54) }, // Apr 24 - Thursday afternoon
        { "92_2025", CreateNaturalDateTime(2025, 4, 25, 9, 46, 12) }, // Apr 25 - Friday morning
        { "93_2025", CreateNaturalDateTime(2025, 4, 28, 16, 33, 38) }, // Apr 28 - Monday afternoon
        { "94_2025", CreateNaturalDateTime(2025, 4, 29, 12, 9, 25) }, // Apr 29 - Tuesday afternoon
        { "95_2025", CreateNaturalDateTime(2025, 4, 29, 18, 42, 51) }, // Apr 29 - Tuesday evening (2nd order)
        { "96_2025", CreateNaturalDateTime(2025, 4, 30, 11, 24, 17) }, // Apr 30 - Wednesday morning
        
        // May25 - Peak and decline pattern
        { "97_2025", CreateNaturalDateTime(2025, 5, 1, 15, 18, 44) }, // May 1 - Thursday afternoon
        { "98_2025", CreateNaturalDateTime(2025, 5, 2, 10, 57, 32) }, // May 2 - Friday morning
        { "99_2025", CreateNaturalDateTime(2025, 5, 3, 17, 29, 19) }, // May 3 - Saturday afternoon
        { "100_2025", CreateNaturalDateTime(2025, 5, 4, 13, 41, 56) }, // May 4 - Sunday afternoon
        { "101_2025", CreateNaturalDateTime(2025, 5, 5, 9, 15, 28) }, // May 5 - Monday morning
        { "102_2025", CreateNaturalDateTime(2025, 5, 5, 16, 48, 7) }, // May 5 - Monday afternoon (2nd order - PEAK)
        { "103_2025", CreateNaturalDateTime(2025, 5, 5, 20, 33, 14) }, // May 5 - Monday evening (3rd order - PEAK)
        { "104_2025", CreateNaturalDateTime(2025, 5, 6, 12, 22, 49) }, // May 6 - Tuesday morning
        { "105_2025", CreateNaturalDateTime(2025, 5, 7, 14, 56, 33) }, // May 7 - Wednesday afternoon
        { "106_2025", CreateNaturalDateTime(2025, 5, 8, 11, 7, 18) }, // May 8 - Thursday morning
        { "107_2025", CreateNaturalDateTime(2025, 5, 9, 16, 24, 42) }, // May 9 - Friday afternoon
        { "108_2025", CreateNaturalDateTime(2025, 5, 12, 10, 39, 27) }, // May 12 - Monday morning
        { "109_2025", CreateNaturalDateTime(2025, 5, 12, 17, 51, 8) }, // May 12 - Monday afternoon (2nd order)
        { "110_2025", CreateNaturalDateTime(2025, 5, 13, 13, 14, 55) }, // May 13 - Tuesday afternoon
        { "111_2025", CreateNaturalDateTime(2025, 5, 14, 9, 28, 41) }, // May 14 - Wednesday morning
        { "112_2025", CreateNaturalDateTime(2025, 5, 15, 15, 47, 23) }, // May 15 - Thursday afternoon
        { "113_2025", CreateNaturalDateTime(2025, 5, 16, 11, 33, 16) }, // May 16 - Friday morning
        { "114_2025", CreateNaturalDateTime(2025, 5, 19, 14, 12, 48) }, // May 19 - Monday afternoon
        { "115_2025", CreateNaturalDateTime(2025, 5, 20, 10, 45, 32) }, // May 20 - Tuesday morning
        { "116_2025", CreateNaturalDateTime(2025, 5, 21, 16, 28, 7) }, // May 21 - Wednesday afternoon
        { "117_2025", CreateNaturalDateTime(2025, 5, 22, 12, 51, 24) }, // May 22 - Thursday afternoon
        { "118_2025", CreateNaturalDateTime(2025, 5, 23, 9, 17, 39) }, // May 23 - Friday morning
        { "119_2025", CreateNaturalDateTime(2025, 5, 26, 13, 44, 52) }, // May 26 - Monday afternoon
        { "120_2025", CreateNaturalDateTime(2025, 5, 27, 11, 6, 18) }, // May 27 - Tuesday morning
        { "121_2025", CreateNaturalDateTime(2025, 5, 28, 15, 39, 41) }, // May 28 - Wednesday afternoon
        { "122_2025", CreateNaturalDateTime(2025, 5, 29, 10, 22, 57) }, // May 29 - Thursday morning
        { "123_2025", CreateNaturalDateTime(2025, 5, 30, 17, 15, 33) }, // May 30 - Friday afternoon
        
        // June25 - Continued natural distribution
        { "124_2025", CreateNaturalDateTime(2025, 6, 2, 12, 48, 19) }, // Jun 2 - Monday afternoon
        { "125_2025", CreateNaturalDateTime(2025, 6, 3, 9, 31, 44) }, // Jun 3 - Tuesday morning
        { "126_2025", CreateNaturalDateTime(2025, 6, 4, 14, 26, 58) }, // Jun 4 - Wednesday afternoon
        { "127_2025", CreateNaturalDateTime(2025, 6, 5, 11, 53, 27) }, // Jun 5 - Thursday morning
        { "128_2025", CreateNaturalDateTime(2025, 6, 6, 16, 7, 12) }, // Jun 6 - Friday afternoon
        { "129_2025", CreateNaturalDateTime(2025, 6, 9, 10, 34, 48) }, // Jun 9 - Monday morning
        { "130_2025", CreateNaturalDateTime(2025, 6, 10, 15, 19, 36) }, // Jun 10 - Tuesday afternoon
        { "131_2025", CreateNaturalDateTime(2025, 6, 11, 13, 42, 21) }, // Jun 11 - Wednesday afternoon
        { "132_2025", CreateNaturalDateTime(2025, 6, 12, 9, 58, 14) }, // Jun 12 - Thursday morning
        { "133_2025", CreateNaturalDateTime(2025, 6, 13, 17, 24, 47) }, // Jun 13 - Friday afternoon
        { "134_2025", CreateNaturalDateTime(2025, 6, 16, 12, 11, 33) }, // Jun 16 - Monday afternoon
        { "135_2025", CreateNaturalDateTime(2025, 6, 17, 10, 45, 28) }, // Jun 17 - Tuesday morning
        { "136_2025", CreateNaturalDateTime(2025, 6, 18, 14, 38, 52) }, // Jun 18 - Wednesday afternoon
        { "137_2025", CreateNaturalDateTime(2025, 6, 19, 11, 27, 19) }, // Jun 19 - Thursday morning
        { "138_2025", CreateNaturalDateTime(2025, 6, 20, 16, 53, 41) }, // Jun 20 - Friday afternoon
        { "139_2025", CreateNaturalDateTime(2025, 6, 23, 9, 16, 25) }, // Jun 23 - Monday morning
        { "140_2025", CreateNaturalDateTime(2025, 6, 24, 13, 49, 58) }, // Jun 24 - Tuesday afternoon
        { "141_2025", CreateNaturalDateTime(2025, 6, 25, 12, 32, 14) }, // Jun 25 - Wednesday afternoon
        { "142_2025", CreateNaturalDateTime(2025, 6, 26, 10, 7, 46) }, // Jun 26 - Thursday morning
        { "143_2025", CreateNaturalDateTime(2025, 6, 27, 15, 28, 33) }, // Jun 27 - Friday afternoon
        { "144_2025", CreateNaturalDateTime(2025, 6, 30, 11, 54, 17) }, // Jun 30 - Monday morning
        
        // July25 - Final entries
        { "145_2025", CreateNaturalDateTime(2025, 7, 1, 14, 37, 42) }, // Jul 1 - Tuesday afternoon
        { "146_2025", CreateNaturalDateTime(2025, 7, 2, 10, 19, 28) }, // Jul 2 - Wednesday morning
        { "147_2025", CreateNaturalDateTime(2025, 7, 3, 16, 52, 11) }, // Jul 3 - Thursday afternoon
        { "148_2025", CreateNaturalDateTime(2025, 7, 4, 12, 8, 35) }, // Jul 4 - Friday morning
    };
}
