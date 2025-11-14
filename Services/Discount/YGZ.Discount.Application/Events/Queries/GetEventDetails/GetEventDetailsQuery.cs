using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;

namespace YGZ.Discount.Application.Events.Queries.GetEventDetails;

public sealed record GetEventDetailsQuery : IQuery<EventResponse>
{
    public required string EventId { get; init; }
}
