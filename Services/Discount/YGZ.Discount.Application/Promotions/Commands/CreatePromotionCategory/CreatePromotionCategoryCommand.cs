
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Discount.Application.Promotions.Commands.CreatePromotionCategory;

public sealed record CreatePromotionCategoryCommand(List<PromotionCategoryCommand> PromotionCategories) : ICommand<bool> { }

public sealed record PromotionCategoryCommand()
{
    public string CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string CategorySlug { get; set; }
    public decimal DiscountPercent { get; set; }
    public string PromotionGlobalId { get; set; }
}
