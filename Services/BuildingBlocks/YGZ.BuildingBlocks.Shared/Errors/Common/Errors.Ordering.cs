namespace YGZ.BuildingBlocks.Shared.Errors.Common;

public static partial class Errors
{
    public static class OrderingGrpc
    {
        public static Error CannotUpdateReviewOrderItem = Error.BadRequest(code: "Ordering.CannotUpdateReviewOrderItem", message: "Cannot update review order item", serviceName: "OrderingService");
    }
}