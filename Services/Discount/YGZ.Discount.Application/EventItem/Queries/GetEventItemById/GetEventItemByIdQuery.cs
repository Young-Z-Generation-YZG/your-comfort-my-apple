using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;

namespace YGZ.Discount.Application.EventItem.Queries.GetEventItemById;

public record GetEventItemByIdQuery(string EventItemId) : IQuery<EventItemResponse>;
