using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class Sku
    {
        public static readonly Error NotFound = Error.NotFound(
            code: "Sku.NotFound",
            message: "The SKU with the specified identifier was not found.",
            serviceName: "CatalogService");
            
        public static readonly Error InsufficientStock = Error.Validation(
            code: "Sku.InsufficientStock",
            message: "The SKU available in stock is not enough.",
            serviceName: "CatalogService");
    }
}
