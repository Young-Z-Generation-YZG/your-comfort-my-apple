
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Core.Enums;

namespace YGZ.Discount.Application.Coupons.Commands.CreatePromotionItem;

public sealed record class CreatePromotionItemCommand() : ICommand<bool>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public NameTag NameTag { get; set; }
    public DiscountState State { get; set; }
    public DiscountType Type { get; set; }
    public decimal DiscountValue { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public int AvailableQuantity { get; set; }
    public string ProductId { get; set; }
    public string ProductSlug { get; set; }
    public string ProductImage { get; set; }
}
