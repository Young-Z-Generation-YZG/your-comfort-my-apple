

using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Ordering.Domain.Core.Errors;

public static partial class Errors
{
    public static class Payment
    {
        public static Error PaymentFailure = Error.BadRequest(code: "Order.PaymentFailure", message: "IPN check payment processing failure", serviceName: "OrderingService");
    }
}