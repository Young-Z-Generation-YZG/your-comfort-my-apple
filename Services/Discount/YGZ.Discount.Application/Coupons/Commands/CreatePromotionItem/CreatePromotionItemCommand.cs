
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Core.Enums;

namespace YGZ.Discount.Application.Coupons.Commands.CreatePromotionItem;

public sealed record class CreatePromotionItemCommand() : ICommand<bool>
{
    required public string ProductId { get; set; }
    required public string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    required public ProductNameTag ProductNameTag { get; set; }
    required public DiscountState DiscountState { get; set; }
    required public DiscountType DiscountType { get; set; }
    required public EndDiscountType EndDiscountType { get; set; }
    required public decimal DiscountValue { get; set; }
    public DateTime? ValidFrom { get; set; } = null;
    public DateTime? ValidTo { get; set; } = null;
    required public int AvailableQuantity { get; set; }
    required public string ProductImage { get; set; }
    required public string ProductSlug { get; set; }
}
