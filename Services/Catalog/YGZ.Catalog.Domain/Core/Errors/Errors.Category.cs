

using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class Category
    {
        public static Error DoesNotExist = Error.BadRequest(code: "Category.DoesNotExist", message: "Category does not exists", serviceName: "CatalogService");
        public static Error ParentDoesNotExist = Error.BadRequest(code: "Category.ParentCategoryDoesNotExist", message: "Parent category does not exists", serviceName: "CatalogService");
    }
}