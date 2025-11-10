using YGZ.BuildingBlocks.Shared.Domain.Core.Abstractions;
using YGZ.Ordering.Application.Orders;

namespace YGZ.Ordering.Domain.Orders.Events;

public sealed record OrderDeliveredDomainEvent(Order Order) : IDomainEvent { }
