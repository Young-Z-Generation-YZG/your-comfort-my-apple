

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.Discount.Domain.Core.Enums;

namespace YGZ.Discount.Application.PromotionCoupons.Commands.CreatePromotionEvent;

public sealed record CreatePromotionEventCommand() : ICommand<bool> 
{
    required public string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    required public DiscountState State { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
}
