
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;

namespace YGZ.Discount.Application.Promotions.Queries.GetPromotionItem;

public sealed record GetPromotionItemsQuery : IQuery<List<PromotionItemResponse>> { }