

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Core.Enums;

namespace YGZ.Discount.Application.Coupons.Commands.UpdateCoupon;

public sealed record UpdateCouponCommand() : ICommand<bool>
{
    required public string Id { get; set; }
    public string? Code { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ProductNameTag? ProductNameTag { get; set; }
    public DiscountState? DiscountState { get; set; }
    public DiscountType? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }
    public decimal? MaxDiscountAmount { get; set; }
    public int? AvailableQuantity { get; set; }
    public DateTime? ValidFrom { get; set; } = null;
    public DateTime? ValidTo { get; set; } = null;
}