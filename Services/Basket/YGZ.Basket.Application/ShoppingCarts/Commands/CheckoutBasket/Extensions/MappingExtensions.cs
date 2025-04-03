
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
            CartItems = cartItems.Select(x => new OrderLineIntegrationEvent(ProductId: x.ProductId,
                                                                            ProductName: x.ProductName,
                                                                            ProductColorName: x.ProductColorName,
                                                                            ProductUnitPrice: x.ProductUnitPrice,
                                                                            ProductNameTag: x.ProductNameTag,
                                                                            ProductImage: x.ProductImage,
                                                                            ProductSlug: x.ProductSlug,
                                                                            Quantity: x.Quantity)).ToList()
        };
    }
}