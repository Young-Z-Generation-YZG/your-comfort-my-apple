
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Core.Enums;

namespace YGZ.Discount.Application.Coupons.Commands.CreatePromotionItem;

public sealed record class CreatePromotionItemCommand() : ICommand<bool>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public ProductNameTag ProductNameTag { get; set; }
    public DiscountState DiscountState { get; set; }
    public DiscountType DiscountType { get; set; }
    public EndDiscountType EndDiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public int? AvailableQuantity { get; set; }
    public string ProductModel { get; set; }
    public int ProductStorage { get; set; }
    public string ProductImage { get; set; }
    public string ProductSlug { get; set; }
}
