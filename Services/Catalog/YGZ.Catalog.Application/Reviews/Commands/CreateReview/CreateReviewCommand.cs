
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Reviews.Commands;

public sealed record CreateReviewCommand : ICommand<bool>
{
    public required string SkuId { get; init; }
    public required string OrderId { get; init; }
    public required string OrderItemId { get; init; }
    public required string Content { get; init; }
    public required int Rating { get; init; }

}
