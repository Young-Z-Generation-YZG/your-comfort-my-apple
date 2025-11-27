using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Discount.Application.Events.Commands.DeductEventItemQuantity;

public class DeductEventItemQuantityCommand : ICommand<bool>
{
    public required string EventItemId { get; init; }
    public required string EventId { get; init; }
    public required int DeductQuantity { get; init; }
}
