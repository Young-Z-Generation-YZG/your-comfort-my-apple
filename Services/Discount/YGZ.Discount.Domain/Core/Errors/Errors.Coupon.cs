
using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Discount.Domain.Core.Errors;

public static partial class Errors
{
    public static class Coupon
    {
        public static Error NotFound = Error.BadRequest(code: "Coupon.NotFound", message: "Coupon not found", serviceName: "DiscountService");
        public static Error OutOfStock = Error.BadRequest(code: "Coupon.OutOfStock", message: "Coupon out of stock", serviceName: "DiscountService");
        public static Error Expired = Error.BadRequest(code: "Coupon.Expired", message: "Coupon has expired", serviceName: "DiscountService");
        public static Error Inactive = Error.BadRequest(code: "Coupon.Inactive", message: "Coupon is not active", serviceName: "DiscountService");
        public static Error InvalidDiscountType = Error.BadRequest(code: "Coupon.InvalidDiscountType", message: "Invalid discount type", serviceName: "DiscountService");
        public static Error InvalidProductClassification = Error.BadRequest(code: "Coupon.InvalidProductClassification", message: "Invalid product classification", serviceName: "DiscountService");
        public static Error InvalidStock = Error.BadRequest(code: "Coupon.InvalidStock", message: "Stock must be greater than 0", serviceName: "DiscountService");
        public static Error DuplicateCode = Error.BadRequest(code: "Coupon.DuplicateCode", message: "Coupon code already exists", serviceName: "DiscountService");
        public static Error InvalidMaxDiscountAmount = Error.BadRequest(code: "Coupon.InvalidMaxDiscountAmount", message: "Max discount amount cannot be negative", serviceName: "DiscountService");
    }
}