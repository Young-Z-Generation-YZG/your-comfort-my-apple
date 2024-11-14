
namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class Product
    {
        public static Error IdInvalid = Error.BadRequest(code: "Product.IdInvalid", message: "Product Id is invalid format objectId");
        public static Error DoesNotExist = Error.BadRequest(code: "Product.DoesNotExist", message: "Product does not Exists");
        public static Error CannotBeCreated = Error.BadRequest(code: "Product.CannotBeCreated", message: "Product cannot be created");
    }
}
