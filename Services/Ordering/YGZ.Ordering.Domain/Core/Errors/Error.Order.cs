
namespace YGZ.Ordering.Domain.Core.Errors;

public static partial class Errors
{
    public static class Order
    {
        public static Error IdInvalid = Error.BadRequest(code: "Order.IdInvalid", message: "Order Id is invalid format objectId");
        public static Error DoesNotExist = Error.BadRequest(code: "Order.DoesNotExist", message: "Order does not Exists");
        public static Error CannotBeCreated = Error.BadRequest(code: "Order.CannotBeCreated", message: "Order cannot be created");
        public static Error InvalidOrderStatus = Error.BadRequest(code: "Order.InvalidOrderStatus", message: "Order status is invalid");
        public static Error InvalidPaymentType = Error.BadRequest(code: "Order.InvalidPaymentType", message: "Payment type is invalid");

    }
}
