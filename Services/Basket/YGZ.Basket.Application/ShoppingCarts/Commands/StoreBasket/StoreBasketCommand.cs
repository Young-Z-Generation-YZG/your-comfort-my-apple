using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket;

public sealed record StoreBasketCommand(List<CartItemCommand> CartItems) : ICommand<bool> { }

public sealed record CartItemCommand
{
    public required string ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string ProductColorName { get; set; }
    public required decimal ProductUnitPrice { get; set; }
    public required string ProductNameTag { get; set; }
    public required string ProductImage { get; set; }
    public required string ProductSlug { get; set; }
    public required string CategoryId { get; set; }
    public required int Quantity { get; set; }
    public PromotionCommand? Promotion { get; set; } = null;
    public required int OrderIndex { get; set; } = 0;
}

public sealed record PromotionCommand
{
    public required string PromotionIdOrCode { get; set; }
    public required string PromotionEventType { get; set; }
}