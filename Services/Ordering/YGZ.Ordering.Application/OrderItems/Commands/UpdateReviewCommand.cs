
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.Ordering.Application.OrderItems.Commands;

public sealed record UpdateReviewCommand : ICommand<BooleanRpcResponse>
{
    required public string ReviewId { get; init; }
    required public string OrderItemId { get; init; }
    required public string CustomerId { get; init; }
    required public string ReviewContent { get; init; }
    required public int ReviewStar { get; init; }
    required public bool IsReviewed { get; init; }
}
