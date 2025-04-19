using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket;

public sealed record StoreBasketCommand(List<CartItemCommand> CartItems) : ICommand<bool> { }

public sealed record CartItemCommand
{
    required public string ProductId { get; set; }
    required public string ModelId { get; set; }
    required public string ProductName { get; set; }
    required public string ProductColorName { get; set; }
    required public decimal ProductUnitPrice { get; set; }
    required public string ProductNameTag { get; set; }
    required public string ProductImage { get; set; }
    required public string ProductSlug { get; set; }
    required public string CategoryId { get; set; }
    required public int Quantity { get; set; }
    public PromotionCommand? Promotion { get; set; } = null;
    required public int OrderIndex { get; set; } = 0;
}

public sealed record PromotionCommand
{
    public required string PromotionIdOrCode { get; set; }
    public required string PromotionEventType { get; set; }
}