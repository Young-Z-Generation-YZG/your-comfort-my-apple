
using YGZ.Ordering.Application.Core.Abstractions.Messaging;

namespace YGZ.Ordering.Application.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand() : ICommand<bool> { }