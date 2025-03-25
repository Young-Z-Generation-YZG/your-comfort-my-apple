

using System.ComponentModel.DataAnnotations;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Application.Coupons.Commands.CreateCoupon;

public sealed record CreatePromotionCouponCommand() : ICommand<bool>
{
    public string Title { get; set; }
    public string? Code { get; set; }
    public string Description { get; set; }
    public ProductNameTag ProductNameTag { get; set; }
    public PromotionEventType PromotionEventType { get; set; }
    public DiscountState DiscountState { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal? MaxDiscountAmount { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public int AvailableQuantity { get; set; }
}