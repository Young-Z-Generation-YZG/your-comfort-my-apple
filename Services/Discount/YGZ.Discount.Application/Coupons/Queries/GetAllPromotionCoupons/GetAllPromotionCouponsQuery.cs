﻿

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;

namespace YGZ.Discount.Application.Coupons.Queries.GetAllPromotionCoupons;

public sealed record GetAllPromotionCouponsQuery : IQuery<List<PromotionCouponResponse>> { }
