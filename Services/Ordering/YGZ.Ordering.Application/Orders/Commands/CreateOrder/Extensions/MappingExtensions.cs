

using YGZ.Ordering.Application.Orders.Commands.Common;
using YGZ.Ordering.Domain.Core.Enums;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Commands.CreateOrder.Extensions;

public static class MappingExtension
{
    public static Order ToEntity(this CreateOrderCommand dto, string userId, string userEmail)
    {
        var shippingAddress = Address.Of(contactName: dto.ShippingAddress.ContactName,
                                         contactEmail: userEmail,
                                         contactPhoneNumber: dto.ShippingAddress.ContactPhoneNumber,
                                         addressLine: dto.ShippingAddress.AddressLine,
                                         district: dto.ShippingAddress.District,
                                         province: dto.ShippingAddress.Province,
                                         country: dto.ShippingAddress.Country);

        var order = Order.Create(orderId: OrderId.Create(),
                                customerId: UserId.Of(new Guid(userId)),
                                code: Code.GenerateCode(),
                                status: OrderStatusEnum.PENDING,
                                paymentMethod: PaymentMethodEnum.FromName(dto.PaymentMethod),
                                discountAmount: dto.DiscountAmount,
                                ShippingAddress: shippingAddress);

        foreach (var item in dto.Orders)
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
                                productModel: dto.ProductModel,
                                productColor: dto.ProductColor,
                                productStorage: dto.ProductStorage,
                                productUnitPrice: dto.ProductUnitPrice,
                                productImage: dto.ProductImage,
                                quantity: dto.Quantity);
    }
}