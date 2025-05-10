


using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Reviews.Commands;

public sealed record UpdateReviewCommand() : ICommand<bool>
{
    required public string ReviewId { get; set; }
    required public string Content { get; set; }
    required public int Rating { get; set; }
}
