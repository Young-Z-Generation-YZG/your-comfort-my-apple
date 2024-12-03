

namespace YGZ.Basket.Domain.Core.Errors;

public static partial class Errors
{
    public static class Basket
    {
        public static Error IdInvalid = Error.BadRequest(code: "Basket.IdInvalid", message: "Basket Id is invalid format objectId");
        public static Error DoesNotExist = Error.BadRequest(code: "Basket.DoesNotExist", message: "Basket does not Exists");
        public static Error CannotBeCreated = Error.BadRequest(code: "Basket.CannotBeCreated", message: "Basket cannot be created");

        public static Error InvalidCartLine(string productId, string model, string color, int storage)
        {
            return Error.BadRequest(code: "Basket.InvalidCartLine", message: $"Cart line invalid. ProductId: {productId}, Model: {model}, Color: {color}, Storage: {storage}");
        }
    }
}