
using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Discount.Domain.Core.Errors;

public static partial class Errors
{
    public static class Coupon
    {
        public static Error CouponNotFound = Error.BadRequest(code: "Discount.CouponNotFound", message: "Coupon not found", serviceName: "DiscountService");
    }
}