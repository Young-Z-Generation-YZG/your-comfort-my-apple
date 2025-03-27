

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.Discount.Domain.Core.Enums;

namespace YGZ.Discount.Application.Promotions.Commands.CreatePromotionProduct;

public sealed record CreatePromotionProductCommand(List<PromotionProductCommand> ProductPromotions) : ICommand<bool>
{
}

public sealed record PromotionProductCommand()
{
    required public string ProductId { get; set; }
    required public string ProductImage { get; set; }
    required public string ProductSlug { get; set; }
    required public DiscountType DiscountType { get; set; }
    required public decimal DiscountValue { get; set; }
    required public string PromotionGlobalId { get; set; }
}
