using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Discount.Application.Coupons.Commands.CreateCoupon;

public sealed record CreateCouponCommand : ICommand<bool>
{
    public string? UserId { get; init; }
    public required string CouponCode { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string ProductClassification { get; init; }
    public required string DiscountType { get; init; }
    public required double DiscountValue { get; init; }
    public double? MaxDiscountAmount { get; init; }
    public required int Stock { get; init; }
    public DateTime? ExpiredDate { get; init; }
}