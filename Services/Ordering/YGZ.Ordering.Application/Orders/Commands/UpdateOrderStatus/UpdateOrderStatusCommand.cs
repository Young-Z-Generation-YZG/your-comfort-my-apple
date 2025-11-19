
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Ordering.Application.Orders.Commands.UpdateOrderStatus;

public sealed record UpdateOrderStatusCommand : ICommand<bool>
{
    public required string OrderId { get; init; }
    public required string UpdateStatus { get; init; }
}
