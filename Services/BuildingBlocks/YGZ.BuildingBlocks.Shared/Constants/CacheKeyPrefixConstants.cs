using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.BuildingBlocks.Shared.Constants;

public static class CacheKeyPrefixConstants
{
    public static class CatalogService
    {
        public static string GetIphoneSkuPriceKey(EIphoneModel modelName, EStorage storageName, EColor colorName)
        {
            return $"IPHONE_SKU_PRICE_{modelName.Name}_{storageName.Name}_{colorName.Name}";
        }
    }
}
