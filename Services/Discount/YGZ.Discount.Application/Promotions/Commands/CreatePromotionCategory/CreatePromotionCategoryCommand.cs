
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.Discount.Domain.Core.Enums;

namespace YGZ.Discount.Application.Promotions.Commands.CreatePromotionCategory;

public sealed record CreatePromotionCategoryCommand(List<PromotionCategoryCommand> PromotionCategories) : ICommand<bool> { }

public sealed record PromotionCategoryCommand()
{
    required public string CategoryId { get; set; }
    required public string CategoryName { get; set; }
    required public string CategorySlug { get; set; }
    required public DiscountType DiscountType { get; set; }
    required public decimal DiscountValue { get; set; }
    required public string PromotionGlobalId { get; set; }
}
