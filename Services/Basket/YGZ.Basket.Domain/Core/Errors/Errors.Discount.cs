

using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Basket.Domain.Core.Errors;

public static partial class Errors
{
    public static class Discount
    {
        public static Error CouponNotFound = Error.BadRequest(code: "Coupon.NotFound", message: "Coupon not found", serviceName: "BasketService");
        public static Error InsufficientStock = Error.BadRequest(code: "EventItem.InsufficientStock", message: "Event item has insufficient stock", serviceName: "BasketService");
    }
}