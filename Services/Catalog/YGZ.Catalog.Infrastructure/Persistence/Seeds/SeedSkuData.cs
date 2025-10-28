using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Seeds;

public static class SeedSkuData
{
    public static IEnumerable<SKU> InventorySkus
    {
        get
        {
            ModelId IPHONE_15_MODEL_ID = ModelId.Of("664351e90087aa09993f5ae7");
            TenantId WARE_HOUSE_TENANT_1 = TenantId.Of("664355f845e56534956be32b");
            BranchId WARE_HOUSE_BRANCH_1 = BranchId.Of("664357a235e84033bbd0e6b6");

            Model IPHONE_15 = Model.Create("IPHONE_15", 0);
            Model IPHONE_15_PLUS = Model.Create("IPHONE_15_PLUS", 1);

            Color BLUE = Color.Create("Blue", "#D5DDDF", "iphone-15-finish-select-202309-6-1inch-blue_zgxzmz", 0);
            Color PINK = Color.Create("Pink", "#EBD3D4", "iphone-15-finish-select-202309-6-1inch-pink_j6v96t", 1);
            Color YELLOW = Color.Create("Yellow", "#EDE6C8", "iphone-15-finish-select-202309-6-1inch-yellow_pwviwe", 2);
            Color GREEN = Color.Create("Green", "#D0D9CA", "iphone-15-finish-select-202309-6-1inch-green_yk0ln5", 3);
            Color BLACK = Color.Create("Black", "#4B4F50", "iphone-15-finish-select-202309-6-1inch-black_hhhvfs", 4);

            Storage STORAGE_128 = Storage.Create("128GB", 128, 0);
            Storage STORAGE_256 = Storage.Create("256GB", 256, 1);
            Storage STORAGE_512 = Storage.Create("512GB", 512, 2);
            Storage STORAGE_1024 = Storage.Create("1TB", 1024, 3);

            EProductClassification ClASSIFICATION_IPHONE = EProductClassification.IPHONE;

            return new List<SKU>
            {
                SKU.Create(IPHONE_15_MODEL_ID, WARE_HOUSE_TENANT_1, WARE_HOUSE_BRANCH_1, SkuCode.Create(ClASSIFICATION_IPHONE.Name, IPHONE_15.NormalizedName, STORAGE_128.NormalizedName, BLUE.NormalizedName), ClASSIFICATION_IPHONE, IPHONE_15, BLUE, STORAGE_128, 1000)
            };
        }
    }
}