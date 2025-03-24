

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Discount.Application.Promotions.Commands.CreatePromotionProduct;

public sealed record CreatePromotionProductCommand(List<PromotionProductCommand> ProductPromotions) : ICommand<bool>
{
}

public sealed record PromotionProductCommand()
{
    public string ProductId { get; set; }
    public string ProductColorName { get; set; }
    public int ProductStorage { get; set; }
    public string ProductSlug { get; set; }
    public string ProductImage { get; set; }
    public decimal DiscountPercentage { get; set; }
    public string PromotionGlobalId { get; set; }
}
