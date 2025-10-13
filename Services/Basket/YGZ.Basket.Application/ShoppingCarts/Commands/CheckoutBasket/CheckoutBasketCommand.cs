

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket;

public sealed record CheckoutBasketCommand : ICommand<CheckoutBasketResponse>
{
    public required ShippingAddressCommand ShippingAddress { get; init; }
    public required string PaymentMethod { get; init; }
    public required string? DiscountCode { get; init; }
}

public sealed record ShippingAddressCommand
{
    public required string ContactName { get; init; }
    public required string ContactPhoneNumber { get; init; }
    public required string AddressLine { get; init; }
    public required string District { get; init; }
    public required string Province { get; init; }
    public required string Country { get; init; }
}
