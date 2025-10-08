
using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Discount.Domain.Core.Errors;

public static partial class Errors
{
    public static class Event
    {
        public static Error EventNotFound = Error.BadRequest(code: "Discount.EventNotFound", message: "Event not found", serviceName: "DiscountService");
    }
}