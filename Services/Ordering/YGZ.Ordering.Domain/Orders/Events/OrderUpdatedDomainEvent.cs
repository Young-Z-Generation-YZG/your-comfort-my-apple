

using YGZ.Ordering.Domain.Core.Abstractions;

namespace YGZ.Ordering.Domain.Orders.Events;

public record OrderUpdatedDomainEvent(Order Order) : IDomainEvent;
