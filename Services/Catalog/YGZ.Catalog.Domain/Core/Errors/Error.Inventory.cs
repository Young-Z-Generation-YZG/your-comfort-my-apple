using YGZ.BuildingBlocks.Shared.Errors;


namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class Inventory
    {
        public static Error SkuDoesNotExist = Error.BadRequest(code: "Inventory.SkuDoesNotExist", message: "Sku does not exist", serviceName: "CatalogService");
    }
}