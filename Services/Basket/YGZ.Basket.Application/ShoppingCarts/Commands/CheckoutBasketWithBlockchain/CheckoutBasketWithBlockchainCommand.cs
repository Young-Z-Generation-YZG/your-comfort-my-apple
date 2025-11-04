using YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasketWithBlockchain;

public sealed record CheckoutBasketWithBlockchainCommand : ICommand<CheckoutBasketResponse>
{
    public required string CryptoUUID { get; init; }
    public required ShippingAddressCommand ShippingAddress { get; init; }
    public required string PaymentMethod { get; init; }
    public required string? DiscountCode { get; init; }
    public string? CustomerPublicKey { get; init; }
    public string? Tx { get; init; }
}
