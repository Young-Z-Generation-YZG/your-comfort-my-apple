

using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Ordering.Domain.Core.Errors;

public static partial class Errors
{
    public static class Order
    {
        public static Error DoesNotExist = Error.BadRequest(code: "Order.DoesNotExist", message: "Order does not exists", serviceName: "OrderingService");
        public static Error AlreadyExists = Error.BadRequest(code: "Order.AlreadyExists", message: "Order already exists", serviceName: "OrderingService");
        public static Error CannotBeCreated = Error.BadRequest(code: "Order.CannotBeCreated", message: "Order cannot be created", serviceName: "OrderingService");
        public static Error CannotConfirmOrder = Error.BadRequest(code: "Order.CannotConfirmOrder", message: $"Order cannot be confirmed, must be in status: {EOrderStatus.PENDING.Name}", serviceName: "OrderingService");
        public static Error CannotCancelOrder = Error.BadRequest(code: "Order.CannotCancelOrder", message: $"Order cannot be cancelled, must be in status: {EOrderStatus.PENDING.Name} | {EOrderStatus.CONFIRMED.Name} | {EOrderStatus.PREPARING.Name}", serviceName: "OrderingService");
        public static Error InvalidOrderStatus = Error.BadRequest(code: "Order.InvalidOrderStatus", message: "Invalid order status", serviceName: "OrderingService");
        public static Error InsufficientStock = Error.BadRequest(code: "Order.InsufficientStock", message: "Insufficient stock", serviceName: "OrderingService");
    }
}