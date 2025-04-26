
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Ordering.Application.Orders.Commands.UpdateOrderStatus;

public sealed record UpdateOrderStatusCommand : ICommand<bool>
{
    required public string OrderId { get; init; }
    required public string UpdatedStatus { get; init; }
}
