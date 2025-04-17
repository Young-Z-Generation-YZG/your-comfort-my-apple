using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;

namespace YGZ.Discount.Application.Promotions.Queries.GetPromotionItemById;

public sealed record GetPromotionItemByIdQuery(string PromotionId) : IQuery<PromotionItemResponse> { }
