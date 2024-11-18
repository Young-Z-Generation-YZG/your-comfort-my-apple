

using YGZ.Basket.Application.Core.Abstractions.Messaging;

namespace YGZ.Basket.Application.Baskets.Commands.DeleteBasket;

public sealed record DeleteBasketCommand(string UserId) : ICommand<bool>;
