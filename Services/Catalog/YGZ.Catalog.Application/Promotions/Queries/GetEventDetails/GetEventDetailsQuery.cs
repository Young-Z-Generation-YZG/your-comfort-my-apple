using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;

namespace YGZ.Catalog.Application.Promotions.Queries.GetEventDetails;

public class GetEventDetailsQuery : IQuery<EventResponse>
{
    public required string EventId { get; init; }
}
