
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;

namespace YGZ.Discount.Application.Coupons.Queries.GetByCouponCode;

public sealed record GetCouponByCodeQuery(string Code) : IQuery<GetCouponResponse> { }