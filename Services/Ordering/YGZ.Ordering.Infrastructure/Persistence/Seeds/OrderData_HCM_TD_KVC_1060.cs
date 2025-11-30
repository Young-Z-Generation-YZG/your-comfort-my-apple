using YGZ.BuildingBlocks.Shared.Contracts.Products;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Infrastructure.Persistence.Seeds;

public static class OrderData_HCM_TD_KVC_1060
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
                               country: "Việt Nam"),
    };
    public static IEnumerable<Order> Orders
    {
        get
        {
            var listUserIds = UserIds.ToList();
            var listShippingAddresses = ShippingAddresses.ToList();
            var listTenantId = ListTenantIds.TenantIds.ToList();
            var listBranchIds = ListBranchIds.BranchIds.ToList();
            var iPhone15ModelsList = ListIphoneModels.iPhone15Models.ToList();
            var iPhone15ColorsList = ListIphoneColors.iPhone15Colors.ToList();
            var iPhone16ModelsList = ListIphoneModels.iPhone16Models.ToList();
            var iPhone16ColorsList = ListIphoneColors.iPhone16Colors.ToList();
            var iPhone16EModelsList = ListIphoneModels.iPhone16EModels.ToList();
            var iPhone16EColorsList = ListIphoneColors.iPhone16EColors.ToList();
            var iPhone17ModelsList = ListIphoneModels.iPhone17Models.ToList();
            var iPhone17ColorsList = ListIphoneColors.iPhone17Colors.ToList();
            var iPhone17ProModelsList = ListIphoneModels.iPhone17ProModels.ToList();
            var iPhone17ProColorsList = ListIphoneColors.iPhone17ProColors.ToList();
            var iPhone17AirModelsList = ListIphoneModels.iPhone17AirModels.ToList();
            var iPhone17AirColorsList = ListIphoneColors.iPhone17AirColors.ToList();
            var StoragesList = Storages.ToList();

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
                { "SILVER", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-17-finish-select-202509-silver_abc123.webp" },
                { "SPACE_BLACK", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-17-finish-select-202509-spaceblack_def456.webp" },
                { "ROSE_GOLD", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-17-finish-select-202509-rosegold_ghi789.webp" }
            };

            var iphone17ProColorImageUrls = new Dictionary<string, string>
            {
                { "TITANIUM_BLACK", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-17-pro-finish-select-202509-titaniumblack_jkl012.webp" },
                { "TITANIUM_WHITE", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960358/iphone-17-pro-finish-select-202509-titaniumwhite_mno345.webp" },
                { "TITANIUM_BLUE", "https://res.cloudinary.com/delkyrtji/image/upload/v1744960389/iphone-17-pro-finish-select-202509-titaniumblue_pqr678.webp" }
            };

            var iphone17AirColorImageUrls = new Dictionary<string, string>
            {
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

            // Create order items - cycling through different models, colors, and storages
            // We'll create 298 order items to match the 298 datetime entries
            var allModels = new List<(List<string> models, List<string> colors, string modelId)>
            {
                (iPhone15ModelsList, iPhone15ColorsList, ListModelIds.ModelIds["IPHONE_15"]),
                (iPhone16ModelsList, iPhone16ColorsList, ListModelIds.ModelIds["IPHONE_16"]),
                (iPhone16EModelsList, iPhone16EColorsList, ListModelIds.ModelIds["IPHONE_16E"]),
                (iPhone17ModelsList, iPhone17ColorsList, ListModelIds.ModelIds["IPHONE_17"]),
                (iPhone17ProModelsList, iPhone17ProColorsList, ListModelIds.ModelIds["IPHONE_17_PRO"]),
                (iPhone17AirModelsList, iPhone17AirColorsList, ListModelIds.ModelIds["IPHONE_17_AIR"])
            };

            var orderItemsLists = new List<List<OrderItem>>();
            var itemIndex = 0;

            for (int i = 1; i <= 298; i++)
            {
                var modelGroup = allModels[itemIndex % allModels.Count];
                var modelIndex = (itemIndex / allModels.Count) % modelGroup.models.Count;
                var colorIndex = (itemIndex / (allModels.Count * modelGroup.models.Count)) % modelGroup.colors.Count;
                var storageIndex = (itemIndex / (allModels.Count * modelGroup.models.Count * modelGroup.colors.Count)) % StoragesList.Count;

                var model = modelGroup.models[modelIndex];
                var color = modelGroup.colors[colorIndex];
                var storage = StoragesList[storageIndex];
                var datetimeKey = $"HCM_TD_KVC_1060_{i}_2025";

                var orderItem = OrderItem.Create(
                    orderItemId: OrderItemId.Create(),
                    tenantId: TenantId.Of(listTenantId[1]), // HCM_TD_KVC_1060
                    branchId: BranchId.Of(listBranchIds[1]), // HCM_TD_KVC_1060
                    orderId: OrderId.Create(),
                    skuId: "SEED_DATA",
                    modelId: modelGroup.modelId,
                    modelName: model,
                    colorName: color,
                    storageName: storage,
                    unitPrice: GetPriceFromSkuPriceList(model, color, storage),
                    displayImageUrl: GetDisplayImageUrl(model, color, storage),
                    modelSlug: GetModelSlug(model),
                    quantity: 1,
                    promotionId: null,
                    promotionType: null,
                    discountType: null,
                    discountValue: null,
                    isReviewed: false,
                    createdAt: ListDateTimes.CreatedAtDateTimes[datetimeKey]);

                orderItemsLists.Add(new List<OrderItem> { orderItem });
                itemIndex++;
            }

            // Create 298 orders
            var orders = new List<Order>();
            for (int i = 1; i <= 298; i++)
            {
                var datetimeKey = $"HCM_TD_KVC_1060_{i}_2025";
                var order = Order.Create(
                    orderId: OrderId.Create(),
                    tenantId: TenantId.Of(listTenantId[1]), // HCM_TD_KVC_1060
                    branchId: BranchId.Of(listBranchIds[1]), // HCM_TD_KVC_1060
                    customerId: listUserIds[0],
                    customerPublicKey: null,
                    tx: null,
                    code: Code.GenerateCode(),
                    paymentMethod: EPaymentMethod.COD,
                    orderStatus: EOrderStatus.DELIVERED,
                    shippingAddress: listShippingAddresses[0],
                    promotionId: null,
                    promotionType: null,
                    discountType: null,
                    discountValue: null,
                    createdAt: ListDateTimes.CreatedAtDateTimes[datetimeKey]);

                order.AddOrderItem(orderItemsLists[i - 1][0]);
                orders.Add(order);
            }

            return orders;
        }
    }

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

    public static IEnumerable<SkuPriceListResponse> SkuPriceList
    {
        get
        {
            var prices = new List<SkuPriceListResponse>();

            // Use the static IEnumerable properties
            var iPhone15ModelsList = ListIphoneModels.iPhone15Models.ToList();
            var iPhone16ModelsList = ListIphoneModels.iPhone16Models.ToList();
            var iPhone16EModelsList = ListIphoneModels.iPhone16EModels.ToList();
            var iPhone17ModelsList = ListIphoneModels.iPhone17Models.ToList();
            var iPhone17ProModelsList = ListIphoneModels.iPhone17ProModels.ToList();
            var iPhone17AirModelsList = ListIphoneModels.iPhone17AirModels.ToList();

            var iPhone15ColorsList = ListIphoneColors.iPhone15Colors.ToList();
            var iPhone16ColorsList = ListIphoneColors.iPhone16Colors.ToList();
            var iPhone16EColorsList = ListIphoneColors.iPhone16EColors.ToList();
            var iPhone17ColorsList = ListIphoneColors.iPhone17Colors.ToList();
            var iPhone17ProColorsList = ListIphoneColors.iPhone17ProColors.ToList();
            var iPhone17AirColorsList = ListIphoneColors.iPhone17AirColors.ToList();

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

}
