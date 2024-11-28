
using YGZ.Basket.Application.Core.Abstractions.Messaging;
using YGZ.Basket.Domain.ShoppingCart.Entities;

namespace YGZ.Basket.Application.Baskets.Commands.StoreBasket;

public sealed record StoreBasketCommand : ICommand<bool> {
    public string UserId { get; set; }
    public string? CouponCode { get; set; }
    public List<CartLineCommand> CartLines { get; set; }
}

public sealed record CartLineCommand(string ProductItemId,
                                     string Model,
                                     string Color,
                                     int Storage,
                                     decimal Price,
                                     int Quantity) { }
