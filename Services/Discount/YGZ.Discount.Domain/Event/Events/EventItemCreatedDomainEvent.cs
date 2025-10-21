using YGZ.BuildingBlocks.Shared.Domain.Core.Abstractions;
using YGZ.Discount.Domain.Event.Entities;

namespace YGZ.Discount.Domain.Event.Events;

public sealed record EventItemCreatedDomainEvent(EventItem EventItem) : IDomainEvent { }