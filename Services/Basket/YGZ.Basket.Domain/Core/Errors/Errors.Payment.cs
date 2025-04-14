

using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Basket.Domain.Core.Errors;

public static partial class Errors
{
    public static class Payment
    {
        public static Error Invalid = Error.BadRequest(code: "Basket.Invalid", message: "Payment invalid", serviceName: "BasketService");
        public static Error MomoPaymentUrlInvalid = Error.BadRequest(code: "Basket.MomoPaymentUrlInvalid", message: "Momo paymentUrl invalid", serviceName: "BasketService");
        public static Error VnpayPaymentUrlInvalid = Error.BadRequest(code: "Basket.VnpayPaymentUrlInvalid", message: "Vnpay paymentUrl invalid", serviceName: "BasketService");
    }
}