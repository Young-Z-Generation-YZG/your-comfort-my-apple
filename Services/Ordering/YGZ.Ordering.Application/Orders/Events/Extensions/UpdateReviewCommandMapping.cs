

using MassTransit;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogServices;
using YGZ.Ordering.Application.OrderItems.Commands;

namespace YGZ.Ordering.Application.Orders.Events.Extensions;

public static class MappingExtensions {
    public static UpdateReviewCommand ToCommand(this ConsumeContext<ReviewCreatedIntegrationEvent> dto)
    {
        return new UpdateReviewCommand
        {
            ReviewId = dto.Message.ReviewId,
            OrderItemId = dto.Message.OrderItemId,
            CustomerId = dto.Message.CustomerId,
            ReviewContent = dto.Message.ReviewContent,
            ReviewStar = dto.Message.ReviewStar,
            IsReviewed = dto.Message.IsReviewed
        };
    }
}