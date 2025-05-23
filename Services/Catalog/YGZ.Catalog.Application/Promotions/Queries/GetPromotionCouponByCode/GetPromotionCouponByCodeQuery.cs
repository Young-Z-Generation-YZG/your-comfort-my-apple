
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;

namespace YGZ.Catalog.Application.Promotions.Queries.GetPromotionCouponByCode;

public sealed record GetPromotionCouponByCodeQuery(string? CouponCode) : IQuery<PromotionCouponResponse> { }
