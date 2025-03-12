

using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Core.Enums;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Infrastructure.Persistence;

public class SeedData
{
    public static IEnumerable<OrderItem> OrderItems => new List<OrderItem>
    {
       OrderItem.Create(OrderItemId.Create(), OrderId.Of(new Guid("3de0c2e6-d082-4d24-b84e-805905674b09")), "67ce96bedf79dc25ff41486e", "iPhone 16", "ultramarine", "#3f51b5", 128, 799, "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-1inch-ultramarine?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4L28rSU1jVGx4VGxCNEFSdVNXdG1RdzJrQmVLSXFrTCsvY1VvVmRlZkVnMzJKTG1lVWJJT2RXQWE0Mm9rU1V0V0E5L1ZBdzY3RU1aTVdUR3lMZHFNVzE0RzhwM3RLeUk1S0YzTkJVVmF2Ly9R&traceId=1", 1)
    };

    public static IEnumerable<Order> Orders
    {
        get
        {
            var address1 = Address.Create("Bach Le", "lov3rinve146@gmail.com", "0333284890", "106* Kha Van Can", "Thu Duc", "Ho Chi Minh", "Viet Nam");

            var order1 = Order.Create(orderId: OrderId.Of(new Guid("3de0c2e6-d082-4d24-b84e-805905674b09")), customerId: UserId.Of(new Guid("d7610ca1-2909-49d3-af23-d502a297da29")), code: Code.Of("#64500665"), status: OrderStatusEnum.PAID, paymentType: PaymentTypeEnum.VNPAY, address1);

            return new List<Order> { order1 };
        }
    }
}
