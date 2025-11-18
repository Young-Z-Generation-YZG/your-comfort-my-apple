using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Promotions.Events.CreateEvent;

public sealed record CreateEventCommand : ICommand<bool>
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
}
