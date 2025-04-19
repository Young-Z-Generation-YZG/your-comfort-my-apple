

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Reviews.Commands;

public sealed record DeleteReviewCommand(string ReviewId) : ICommand<bool> { }