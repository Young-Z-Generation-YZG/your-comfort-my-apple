

using YGZ.Ordering.Application.Orders.Commands.Common;
using YGZ.Ordering.Domain.Core.Enums;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Commands.CreateOrder.Extensions;

public static class MappingExtension
{
    public static Order ToEntity(this CreateOrderCommand dto, Guid orderId, string userId, string userEmail)
    {
        var shippingAddress = Address.Of(contactName: dto.ShippingAddress.ContactName,
                                         contactEmail: userEmail,
                                         contactPhoneNumber: dto.ShippingAddress.ContactPhoneNumber,
                                         addressLine: dto.ShippingAddress.AddressLine,
                                         district: dto.ShippingAddress.District,
                                         province: dto.ShippingAddress.Province,
                                         country: dto.ShippingAddress.Country);

        var order = Order.Create(orderId: OrderId.Of(orderId),
                                 customerId: UserId.Of(new Guid(userId)),
                                 code: Code.GenerateCode(),
                                 status: OrderStatusEnum.PENDING,
                                 paymentMethod: PaymentMethodEnum.FromName(dto.PaymentMethod),
                                 discountAmount: dto.DiscountAmount,
                                 ShippingAddress: shippingAddress);

        foreach (var item in dto.OrderItems)
        {
            order.AddOrderItem(item.ToEntity(order.Id));
        }

        return order;
    }

    public static OrderItem ToEntity(this OrderItemCommand dto, OrderId orderId)
    {
        return OrderItem.Create(orderItemId: OrderItemId.Create(),
                                orderId: orderId,
                                productId: dto.ProductId,
                                modelId: dto.ModelId,
                                productName: dto.ProductName,
                                productColorName: dto.ProductColorName,
                                productUnitPrice: dto.ProductUnitPrice,
                                productImage: dto.ProductImage,
                                productSlug: dto.ProductSlug,
                                quantity: dto.Quantity,
                                promotion: dto.Promotion is not null ? Promotion.Create(promotionIdOrCode: dto.Promotion.PromotionIdOrCode,
                                                                                        promotionEventType: dto.Promotion.PromotionEventType,
                                                                                        promotionTitle: dto.Promotion.PromotionTitle,
                                                                                        promotionDiscountType: dto.Promotion.PromotionDiscountType,
                                                                                        promotionDiscountValue: dto.Promotion.PromotionDiscountValue,
                                                                                        promotionDiscountUnitPrice: dto.Promotion.PromotionDiscountUnitPrice,
                                                                                        promotionAppliedProductCount: dto.Promotion.PromotionAppliedProductCount,
                                                                                        promotionFinalPrice: dto.Promotion.PromotionFinalPrice) : null);
    }
}