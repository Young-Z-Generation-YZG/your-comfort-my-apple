
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Reviews.Commands;

public sealed record CreateReviewCommand : ICommand<bool>
{
    required public string ProductId { get; set; }
    required public string ModelId { get; set; }
    required public string OrderItemId { get; set; }
    required public string Content { get; set; }
    required public int Rating { get; set; }
}
