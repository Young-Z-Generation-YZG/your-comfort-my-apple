

using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Ordering.Domain.Core.Errors;

public static partial class Errors
{
    public static class Order
    {
        public static Error DoesNotExist = Error.BadRequest(code: "Order.DoesNotExist", message: "Order does not exists", serviceName: "OrderingService");
        public static Error AlreadyExists = Error.BadRequest(code: "Order.AlreadyExists", message: "Order already exists", serviceName: "OrderingService");
        public static Error CannotBeCreated = Error.BadRequest(code: "Order.CannotBeCreated", message: "Order cannot be created", serviceName: "OrderingService");
    }
}