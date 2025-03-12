

using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Core.Abstractions;

namespace YGZ.Ordering.Domain.Orders.Events;

public record OrderCreatedDomainEvent(Order Order) : IDomainEvent;
