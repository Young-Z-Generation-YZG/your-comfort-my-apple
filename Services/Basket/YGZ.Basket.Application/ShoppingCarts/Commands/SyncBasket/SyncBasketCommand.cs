using YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.SyncBasket;

public sealed record SyncBasketCommand : ICommand<bool>
{
    public required List<CartItemCommand> CartItems { get; init; }
}
