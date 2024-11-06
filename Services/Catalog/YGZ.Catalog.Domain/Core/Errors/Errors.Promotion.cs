
namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class Promotion
    {
        public static Error PromotionDoesNotExist = Error.BadRequest(code: "Promotion.PromotionDoesNotExist", message: "Promotion does not Exists");
        public static Error PromotionCannotBeCreated = Error.BadRequest(code: "Promotion.PromotionCannotBeCreated", message: "Promotion cannot be created");
        public static Error PromotionIdInvalid = Error.BadRequest(code: "Promotion.PromotionIdInvalid", message: "Promotion Id is invalid format objectId");
    }
}
