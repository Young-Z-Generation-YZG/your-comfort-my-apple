
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Reviews.Commands;

public sealed record CreateReviewCommand : ICommand<bool>
{
    public required string ProductId { get; set; }
    public required string ModelId { get; set; }
    public required string OrderId { get; set; }
    public required string OrderItemId { get; set; }
    public required string CustomerUserName { get; set; }
    public required string Content { get; set; }
    public required int Rating { get; set; }
}
