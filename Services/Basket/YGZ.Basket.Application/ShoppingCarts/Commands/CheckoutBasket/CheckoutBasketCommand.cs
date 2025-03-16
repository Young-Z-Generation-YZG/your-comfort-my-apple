

using YGZ.Basket.Application.ShoppingCarts.Commands.Common;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket;

public sealed record CheckoutBasketCommand(ShippingAddressCommand ShippingAddress,
                                           string PaymentMethod,
                                           string? DiscountCode,
                                           decimal DiscountAmount,
                                           decimal SubTotalAmount,
                                           decimal TotalAmount) : ICommand<bool>
{ }

