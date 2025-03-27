

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;

namespace YGZ.Discount.Application.Coupons.Queries.GetAllCoupons;

public sealed record GetAllCouponsQuery() : IQuery<PaginationResponse<GetCouponResponse>> { }