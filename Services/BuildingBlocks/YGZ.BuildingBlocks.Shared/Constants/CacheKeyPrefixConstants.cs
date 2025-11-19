using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.BuildingBlocks.Shared.Constants;

public static class CacheKeyPrefixConstants
{
    public static class CatalogService
    {
        public static string GetIphoneSkuPriceKey(EIphoneModel modelEnum, EColor colorEnum, EStorage storageEnum)
        {
            return $"IPHONE_SKU_PRICE_{modelEnum.Name}_{colorEnum.Name}_{storageEnum.Name}";
        }

        public static string GetDisplayImageUrlKey(string modelId, EColor colorEnum)
        {
            return $"DISPLAY_IMAGE_{modelId}_{colorEnum.Name}";
        }
    }
}
