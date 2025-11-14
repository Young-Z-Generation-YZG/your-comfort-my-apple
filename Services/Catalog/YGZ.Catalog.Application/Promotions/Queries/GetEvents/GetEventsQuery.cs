using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;

namespace YGZ.Catalog.Application.Promotions.Queries.GetEvents;

public sealed record GetEventsQuery : IQuery<List<EventResponse>>
{
}
