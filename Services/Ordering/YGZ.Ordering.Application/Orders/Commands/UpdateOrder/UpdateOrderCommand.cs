

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Ordering.Application.Orders.Commands.UpdateOrder;

public sealed record UpdateOrderCommand() : ICommand<bool>
{
    required public string OrderId { get; init; }
    required public string UpdateStatus { get; init; }
}

