﻿

using YGZ.BuildingBlocks.Shared.Domain.Core.Abstractions;
using YGZ.Ordering.Application.Orders;

namespace YGZ.Ordering.Domain.Orders.Events;

public record OrderCreatedDomainEvent(Order Order) : IDomainEvent;
