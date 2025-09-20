using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Discount.Application.Events.Commands.CreateEvent;

public sealed record CreateEventCommand : ICommand<bool>
{
    public required string Title { get; set; }
    public string? Description { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
