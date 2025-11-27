using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Ordering.Domain.Core.Errors;

public static partial class Errors
{
    public static class Notification
    {
        public static Error NotFound = Error.BadRequest(code: "Notification.NotFound", message: "Notification not found", serviceName: "OrderingService");
        public static Error Unauthorized = Error.BadRequest(code: "Notification.Unauthorized", message: "You are not authorized to access this notification", serviceName: "OrderingService");
    }
}

