using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Discount.Application.Coupons.Commands.CreateCoupon;

public sealed record CreateCouponCommand : ICommand<bool>
{
    public required string UniqueCode { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string CategoryType { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public required decimal MaxDiscountAmount { get; init; }
    public required int AvailableQuantity { get; init; }
    public required int Stock { get; init; }
}