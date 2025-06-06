﻿
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.BasketService;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket.Extensions;

public static class MappingExtension
{
    public static BasketCheckoutIntegrationEvent ToBasketCheckoutIntegrationEvent(this CheckoutBasketCommand dto,
                                                                                  Guid orderId,
                                                                                  string customerId,
                                                                                  string customerEmail,
                                                                                  List<ShoppingCartItem> cartItems,
                                                                                  decimal discountAmount,
                                                                                  decimal subTotalAmount,
                                                                                  decimal totalAmount)
    {
        return new BasketCheckoutIntegrationEvent
        {
            OrderId = orderId,
            CustomerId = customerId,
            CustomerEmail = customerEmail,
            ContactName = dto.ContactName,
            ContactPhoneNumber = dto.ContactPhoneNumber,
            PaymentMethod = dto.PaymentMethod,
            AddressLine = dto.AddressLine,
            District = dto.District,
            Province = dto.Province,
            Country = dto.Country,
            DiscountAmount = discountAmount,
            SubTotalAmount = subTotalAmount,
            TotalAmount = totalAmount,
            OrderItems = cartItems.Select(x => new OrderItemIntegrationEvent()
            {
                ProductId = x.ProductId,
                ModelId = x.ModelId,
                ProductName = x.ProductName,
                ProductColorName = x.ProductColorName,
                ProductUnitPrice = x.ProductUnitPrice,
                ProductNameTag = x.ProductNameTag,
                ProductImage = x.ProductImage,
                ProductSlug = x.ProductSlug,
                Quantity = x.Quantity,
                Promotion = x.Promotion is not null ? new PromotionIntergrationEvent()
                {
                    PromotionIdOrCode = x.Promotion.PromotionIdOrCode,
                    PromotionEventType = x.Promotion.PromotionEventType,
                    PromotionTitle = x.Promotion.PromotionTitle,
                    PromotionDiscountType = x.Promotion.PromotionDiscountType,
                    PromotionDiscountValue = (decimal)(x.Promotion.PromotionDiscountValue ?? 0),
                    PromotionDiscountUnitPrice = (decimal)(x.Promotion.PromotionDiscountUnitPrice ?? 0),
                    PromotionAppliedProductCount = (int)(x.Promotion.PromotionAppliedProductCount ?? 0),
                    PromotionFinalPrice = (decimal)(x.Promotion.PromotionFinalPrice ?? 0)
                } : null
            }).ToList()
        };
    }
}