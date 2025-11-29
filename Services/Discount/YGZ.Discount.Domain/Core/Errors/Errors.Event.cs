
using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Discount.Domain.Core.Errors;

public static partial class Errors
{
    public static class Event
    {
        public static Error EventNotFound = Error.BadRequest(code: "Discount.EventNotFound", message: "Event not found", serviceName: "DiscountService");
    }

    public static class EventItem
    {
        public static Error EventItemNotFound = Error.BadRequest(code: "Discount.EventItemNotFound", message: "Event item not found", serviceName: "DiscountService");
        public static Error InvalidQuantity = Error.BadRequest(code: "Discount.InvalidQuantity", message: "Invalid quantity. Quantity must be greater than zero", serviceName: "DiscountService");
        public static Error InsufficientStock = Error.BadRequest(code: "Discount.InsufficientStock", message: "Cannot increase sold quantity beyond available stock", serviceName: "DiscountService");
    }
}