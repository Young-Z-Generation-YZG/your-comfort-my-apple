
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.BasketService;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket.Extensions;

public static class MappingExtension
{
    public static BasketCheckoutIntegrationEvent ToBasketCheckoutIntegrationEvent(this CheckoutBasketCommand dto,
                                                                                  string customerId,
                                                                                  string customerEmail,
                                                                                  List<ShoppingCartItem> cartItems,
                                                                                  decimal discountAmount,
                                                                                  decimal subTotalAmount,
                                                                                  decimal totalAmount)
    {
        return new BasketCheckoutIntegrationEvent
        {
            CustomerId = customerId,
            CustomerEmail = customerEmail,
            ContactName = dto.ShippingAddress.ContactName,
            ContactPhoneNumber = dto.ShippingAddress.ContactPhoneNumber,
            PaymentMethod = dto.PaymentMethod,
            AddressLine = dto.ShippingAddress.AddressLine,
            District = dto.ShippingAddress.District,
            Province = dto.ShippingAddress.Province,
            Country = dto.ShippingAddress.Country,
            DiscountAmount = dto.DiscountAmount,
            SubTotalAmount = dto.SubTotalAmount,
            TotalAmount = dto.TotalAmount,
            CartItems = cartItems.Select(x => new OrderLineIntegrationEvent(x.ProductId,
                                                                            x.ProductModel,
                                                                            x.ProductColor,
                                                                            x.ProductColorHex,
                                                                            x.ProductStorage,
                                                                            x.ProductPrice,
                                                                            x.ProductImage,
                                                                            x.Quantity)).ToList()
        };
    }
}