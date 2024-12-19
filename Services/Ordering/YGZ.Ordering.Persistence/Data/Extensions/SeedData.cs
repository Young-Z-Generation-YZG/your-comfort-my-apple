

using YGZ.Ordering.Domain.Orders;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;
using static YGZ.Ordering.Domain.Core.Enums.Enums;

namespace YGZ.Ordering.Persistence.Data.Extensions;

internal class SeedData
{
    public static IEnumerable<Customer> Customers => new List<Customer>
    {
        Customer.CreateNew(CustomerId.Of(new Guid("78dc45ca-a007-4d33-9616-2d8e44735e1a")) ,"John Doe", "lxbach1608@gmail.com"),
    };

    public static IEnumerable<Product> Products => new List<Product>
    {
        Product.CreateNew(ProductId.Of("674f20b3ed034b761cd47ec2"),"iPhone 16", "pink", 128, 799),
    };

    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            var address = Address.CreateNew("1060/2, Kha Vạn Cân", "Thủ Đức", "HCM", "VN", "Lê Xuân Bách", "lxbach1608@gmail.com", "0333284890");

            var order = Order.CreateNew(CustomerId.Of(new Guid("78dc45ca-a007-4d33-9616-2d8e44735e1a")), address, address, OrderStatus.PAID, PaymentType.VNPAY);

            order.AddOrderLine(ProductId.Of("674f20b3ed034b761cd47ec2"),
                               "iPhone 16",
                               "pink",
                               128,
                               "https://store.storeimages.cdn-apple.com/1/as-images.apple.com/is/iphone-16-finish-select-202409-6-7inch-ultramarine?wid=5120&hei=2880&fmt=webp&qlt=70&.v=UXp1U3VDY3IyR1hNdHZwdFdOLzg1V0tFK1lhSCtYSGRqMUdhR284NTN4OUp1NDJCalJ6MnpHSm1KdCtRZ0FvSDJrQmVLSXFrTCsvY1VvVmRlZkVnMzJKTG1lVWJJT2RXQWE0Mm9rU1V0V0R6SkNnaG1kYkl1VUVsNXVsVGJrQ0s0UmdXWi9jaTBCeEx5VFNDNXdWbmdB&traceId=1",
                               1,
                               1000,
                               200,
                               800);

            return new List<Order> { order };
        }
    }

}
