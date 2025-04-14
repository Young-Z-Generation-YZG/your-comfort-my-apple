

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

namespace YGZ.Catalog.Application.Promotions.Queries.GetActivePromotionEvent;

public sealed record GetActivePromotionEventQuery() : IQuery<ActivePromotionEventResponse> { }
