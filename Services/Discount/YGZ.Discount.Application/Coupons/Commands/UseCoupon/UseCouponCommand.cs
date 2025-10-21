using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Discount.Application.Coupons.Commands.UpdateCouponQuantity;

public sealed record UseCouponCommand : ICommand<bool>
{
    public required string CouponId { get; init; }
}
