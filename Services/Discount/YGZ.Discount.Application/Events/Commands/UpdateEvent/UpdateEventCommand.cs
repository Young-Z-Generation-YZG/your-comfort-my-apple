using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.Discount.Application.Events.Commands.AddEventItem;

namespace YGZ.Discount.Application.Events.Commands.UpdateEvent;

public sealed record UpdateEventCommand : ICommand<bool>
{
    public required string EventId { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public List<DiscountEventItemCommand>? AddEventItems { get; init; }
    public List<string>? RemoveEventItemIds { get; init; }
}
