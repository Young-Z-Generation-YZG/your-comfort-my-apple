

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Promotions.Commands.CreatePromotionGlobal;

public sealed record CreatePromotionGlobalCommand : ICommand<bool>
{
    public required string GlobalTitle { get; set; }
    public required string GlobalDescription { get; set; }
    public required string GlobalType { get; set; }
    public required string EventId { get; set; }
    public List<PromotionCategoryCommand> PromotionCategories { get; set; } = new();
    public List<PromotionProductCommand> PromotionProducts { get; set; } = new();
}

public sealed record PromotionCategoryCommand
{
    public required string CategoryId { get; set; }
    public required string CategoryName { get; set; }
    public required string CategorySlug { get; set; }
    public required string DiscountType { get; set; }
    public required decimal DiscountValue { get; set; }
}

public sealed record PromotionProductCommand
{
    public required string ProductId { get; set; }
    public required string ProductSlug { get; set; }
    public required string ProductImage { get; set; }
    public required string DiscountType { get; set; }
    public required decimal DiscountValue { get; set; }
}