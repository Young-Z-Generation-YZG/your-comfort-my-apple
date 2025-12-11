using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasketItem;

public class StoreBasketItemCommand : ICommand<bool>
{
    public required string SkuId { get; init; }
    public required int Quantity { get; init; }
}
