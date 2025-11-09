using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Ordering.Application.OrderItems.Commands.UpdateIsReviewed;

public sealed record UpdateIsReviewedCommand : ICommand<bool>
{
    public required string OrderItemId { get; init; }
    public required bool IsReviewed { get; init; }
}

