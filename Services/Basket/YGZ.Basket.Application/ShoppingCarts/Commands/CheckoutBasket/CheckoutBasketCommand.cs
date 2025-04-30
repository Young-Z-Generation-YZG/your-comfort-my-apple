

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket;

public sealed record CheckoutBasketCommand() : ICommand<CheckoutBasketResponse>
{ 
    required public string ContactName { get; set; }
    required public string ContactPhoneNumber { get; set; }
    required public string AddressLine { get; set; }
    required public string District { get; set; }
    required public string Province { get; set; }
    required public string Country { get; set; }
    required public string PaymentMethod { get; set; }
    required public string? DiscountCode { get; set; }
    required public decimal DiscountAmount { get; set; }
    required public decimal SubTotalAmount { get; set; }
    required public decimal TotalAmount { get; set; }
}