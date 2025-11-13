using YGZ.BuildingBlocks.Shared.Domain.Core.Abstractions;
using YGZ.Ordering.Application.Orders;

namespace YGZ.Ordering.Domain.Orders.Events;

public sealed record OrderPaidDomainEvent(Order Order) : IDomainEvent { };