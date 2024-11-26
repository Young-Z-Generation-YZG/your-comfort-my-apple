
namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class Product
    {
        public static Error IdInvalid = Error.BadRequest(code: "Product.IdInvalid", message: "Product Id is invalid format objectId");
        public static Error DoesNotExist = Error.BadRequest(code: "Product.DoesNotExist", message: "Product does not Exists");
        public static Error CannotBeCreated = Error.BadRequest(code: "Product.CannotBeCreated", message: "Product cannot be created");
        public static Error CannotBeAddProductItem = Error.BadRequest(code: "Product.CannotBeAddProductItem", message: "Product cannot be add product item");
        public static Error InvalidStorage = Error.BadRequest(code: "Product.InvalidStorage", message: "Product storage is invalid");

        public static Error InvalidModel(List<string> models)
        {
            var str = $"({string.Join(" | ", models)})";
            return Error.BadRequest(code: "Product.InvalidModel", message: $"Product model invalid. Valid models {str}");
        }
        public static Error InvalidColor(List<string> colors)
        {
            var str = $"({string.Join(" | ", colors)})";
            return Error.BadRequest(code: "Product.InvalidColor", message: $"Product color invalid. Valid colors {str}");
        }
    }
}
