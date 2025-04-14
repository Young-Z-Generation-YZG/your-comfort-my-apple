

using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Basket.Domain.Core.Errors;

public static partial class Errors
{
    public static class Discount
    {
        public static Error PromotionCouponNotFound = Error.BadRequest(code: "BasketDiscountRpc.CouponNotFound", message: "Promotion Coupon not found", serviceName: "BasketDiscountRpc");
    }
}