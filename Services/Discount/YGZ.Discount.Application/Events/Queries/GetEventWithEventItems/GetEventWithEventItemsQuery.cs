using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;

namespace YGZ.Discount.Application.Events.Queries.GetEventWithEventItems;

public sealed record GetEventWithEventItemsQuery : IQuery<EventWithEventItemsResponse> { }
