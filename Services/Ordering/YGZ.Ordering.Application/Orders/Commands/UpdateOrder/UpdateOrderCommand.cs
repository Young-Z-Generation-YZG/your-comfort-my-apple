
using YGZ.Ordering.Application.Core.Abstractions.Messaging;

namespace YGZ.Ordering.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand() : ICommand<bool> { }
