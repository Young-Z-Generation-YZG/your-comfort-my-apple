
namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class Category
    {
        public static Error CategoryDoesNotExist = Error.BadRequest(code: "Category.CategoryDoesNotExist", message: "Category does not Exists");
        public static Error CategoryCannotBeCreated = Error.BadRequest(code: "Category.CategoryCannotBeCreated", message: "Category cannot be created");
        public static Error CategoryIdInvalid = Error.BadRequest(code: "Category.CategoryIdInvalid", message: "Category Id is invalid format objectId");
    }
}