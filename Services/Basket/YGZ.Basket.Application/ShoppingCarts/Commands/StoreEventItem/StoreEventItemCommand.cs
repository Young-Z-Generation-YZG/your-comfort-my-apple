using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreEventItem;

public sealed record StoreEventItemCommand : ICommand<bool>
{
    public required string EventItemId { get; init; }
}