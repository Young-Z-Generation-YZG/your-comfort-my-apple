using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Basket.Domain.Core.Errors;

public static partial class Errors
{
    public static class Catalog
    {
        public static Error SkuNotFound = Error.BadRequest(code: "Sku.NotFound", message: "Sku not found", serviceName: "BasketService");
    }
}