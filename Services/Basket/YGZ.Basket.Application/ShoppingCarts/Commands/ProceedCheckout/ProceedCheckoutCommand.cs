using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.ProceedCheckout;

public sealed record ProceedCheckoutCommand() : ICommand<bool> { }
