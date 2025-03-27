
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Discount.Application.Coupons.Commands.DeleteCoupon;

public sealed record DeleteCouponCommand(string CouponId) : ICommand<bool> { }