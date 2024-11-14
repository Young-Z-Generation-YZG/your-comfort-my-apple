

namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class ProductItem
    {
        public static Error IdInvalid = Error.BadRequest(code: "ProductItem.IdInvalid", message: "ProductItem Id is invalid format objectId");
        public static Error DoesNotExist = Error.BadRequest(code: "ProductItem.DoesNotExist", message: "ProductItem does not Exists");
        public static Error CannotBeCreated = Error.BadRequest(code: "ProductItem.CannotBeCreated", message: "ProductItem cannot be created");
    }
}
