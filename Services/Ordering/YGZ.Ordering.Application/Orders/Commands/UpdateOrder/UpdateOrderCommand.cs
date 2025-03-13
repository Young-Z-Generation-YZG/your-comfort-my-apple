

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Ordering.Application.Orders.Commands.UpdateOrder;

public sealed record UpdateOrderCommand() : ICommand<bool>;

