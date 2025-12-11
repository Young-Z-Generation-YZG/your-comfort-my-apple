using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket;

public sealed record StoreBasketCommand(List<CartItemCommand> CartItems) : ICommand<bool> { }

public sealed record CartItemCommand
{
    public required bool IsSelected { get; init; }
    public required string SkuId { get; init; }
    public required int Quantity { get; init; }
}
