
using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class Coupon
    {
        public static Error PromotionCodeDoesNotExist = Error.BadRequest(code: "Discount.PromotionCodeDoesNotExist", message: "Promotion code does not exists", serviceName: "CatalogService");
        public static Error PromotionCodeNotProvided = Error.BadRequest(code: "Discount.PromotionCodeNotProvided", message: "Promotion code not provided", serviceName: "CatalogService");
        public static Error PromotionCodeExpired = Error.BadRequest(code: "Discount.PromotionCodeExpired", message: "Promotion code expired", serviceName: "CatalogService");

    }
}