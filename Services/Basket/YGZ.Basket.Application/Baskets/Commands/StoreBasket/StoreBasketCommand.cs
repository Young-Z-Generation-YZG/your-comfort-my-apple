
using YGZ.Basket.Application.Core.Abstractions.Messaging;
using YGZ.Basket.Domain.ShoppingCart.Entities;

namespace YGZ.Basket.Application.Baskets.Commands.StoreBasket;

public record StoreBasketCommand : ICommand<bool> {
    public string UserId { get; set; }
    public string? CouponCode { get; set; }
    public List<CartLineCommand> CartLines { get; set; }
}

public record CartLineCommand(string ProductItemId,
                                     string Model,
                                     string Color,
                                     int Storage,
                                     decimal Price,
                                     int Quantity) { }
