
using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Discount.Domain.Core.Errors;

public static partial class Errors
{
    public static class Coupon
    {
        public static Error NotFound = Error.BadRequest(code: "Coupon.NotFound", message: "Coupon not found", serviceName: "DiscountService");
        public static Error OutOfStock = Error.BadRequest(code: "Coupon.OutOfStock", message: "Coupon out of stock", serviceName: "DiscountService");
    }
}