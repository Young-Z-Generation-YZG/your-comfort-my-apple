using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Discount.Application.Coupons.Commands.UseCoupon;

public sealed record UseCouponCommand : ICommand<bool>
{
    public required string CouponCode { get; init; }
}
