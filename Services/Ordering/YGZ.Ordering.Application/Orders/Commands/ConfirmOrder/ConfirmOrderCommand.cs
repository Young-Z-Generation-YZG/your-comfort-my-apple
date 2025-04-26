

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Ordering.Application.Orders.Commands.ConfirmOrder;

public sealed record ConfirmOrderCommand(string OrderId) : ICommand<bool> { }
