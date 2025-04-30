

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;

namespace YGZ.Catalog.Application.Promotions.Queries.GetPromotionEvents;

public sealed record GetPromotionEventsQuery : IQuery<PaginationResponse<PromotionEventResponse>> { }