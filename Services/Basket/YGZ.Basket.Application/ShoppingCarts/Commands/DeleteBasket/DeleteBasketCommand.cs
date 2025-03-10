
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.DeleteBasket;

public sealed record DeleteBasketCommand() : ICommand<bool> { }
