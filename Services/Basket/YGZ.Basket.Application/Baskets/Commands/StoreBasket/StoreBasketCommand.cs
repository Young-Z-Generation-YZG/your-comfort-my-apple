﻿
using YGZ.Basket.Application.Core.Abstractions.Messaging;
using YGZ.Basket.Domain.ShoppingCart.Entities;

namespace YGZ.Basket.Application.Baskets.Commands.StoreBasket;

public sealed record StoreBasketCommand : ICommand<bool> {
    public string UserId { get; set; }
    public List<CartLineCommand> CartLines { get; set; }
}

public sealed record CartLineCommand(string ProductId,
                                     string Sku,
                                     string Model,
                                     string Color,
                                     string Storage,
                                     int Price,
                                     int Quantity,
                                     string ImageUrl) { }