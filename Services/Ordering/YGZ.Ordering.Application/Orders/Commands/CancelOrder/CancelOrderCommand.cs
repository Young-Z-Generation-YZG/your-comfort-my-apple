

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Ordering.Application.Orders.Commands.CancelOrder;

public sealed record CancelOrderCommand(string OrderId) : ICommand<bool> { }