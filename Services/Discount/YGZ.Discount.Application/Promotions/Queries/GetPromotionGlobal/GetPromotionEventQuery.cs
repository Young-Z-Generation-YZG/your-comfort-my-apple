
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Domain.Core.Enums;

namespace YGZ.Discount.Application.Promotions.Queries.GetPromotionGlobal;

public sealed record GetPromotionEventQuery() : IQuery<List<PromotionGlobalEventResponse>> { }
