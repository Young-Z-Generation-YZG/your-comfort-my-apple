

using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Basket.Domain.Core.Errors;

public static partial class Errors
{
    public static class Discount
    {
        public static Error CouponNotFound = Error.BadRequest(code: "Coupon.NotFound", message: "Coupon not found", serviceName: "DiscountService");
    }
}