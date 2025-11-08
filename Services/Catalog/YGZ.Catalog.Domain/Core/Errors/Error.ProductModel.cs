using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class ProductModel
    {
        public static Error DoesNotExist = Error.BadRequest(code: "ProductModel.DoesNotExist", message: "Product model does not exist", serviceName: "CatalogService");
    }
}