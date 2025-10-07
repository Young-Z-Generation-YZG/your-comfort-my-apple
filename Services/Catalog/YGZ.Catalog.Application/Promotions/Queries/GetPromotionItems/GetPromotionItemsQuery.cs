

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Promotions;

namespace YGZ.Catalog.Application.Promotions.Queries.GetPromotionItems;

public sealed record GetPromotionItemsQuery : IQuery<PromotionIphoneItemEventResponse> { }