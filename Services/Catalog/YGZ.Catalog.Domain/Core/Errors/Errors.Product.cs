
namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class Product
    {
        public static Error ProductDoesNotExist = Error.BadRequest(code: "Product.ProductDoesNotExist", message: "Product does not Exists");
        public static Error ProductCannotBeCreated = Error.BadRequest(code: "Product.ProductCannotBeCreated", message: "Product cannot be created");
    }
}
