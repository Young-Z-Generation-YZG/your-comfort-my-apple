

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Ordering.Application.Orders.Commands.DeleteOrder;

public sealed record DeleteOrderCommand() : ICommand<bool>;

