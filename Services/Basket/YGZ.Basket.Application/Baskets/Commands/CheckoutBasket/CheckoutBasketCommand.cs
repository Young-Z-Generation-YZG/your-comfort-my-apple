


using YGZ.Basket.Application.Core.Abstractions.Messaging;

namespace YGZ.Basket.Application.Baskets.Commands.CheckoutBasket;

public record CheckoutBasketCommand(string UserId, string FirstName, string LastName) : ICommand<bool> { }
