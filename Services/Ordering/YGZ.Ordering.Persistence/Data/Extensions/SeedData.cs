

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
        Product.CreateNew(ProductId.Of("78dc45ca"),"Iphone 16", "iPhone 16", "pink", "256", 1000, 3),
        Product.CreateNew(ProductId.Of("78dc45cb"),"Samsung", "Samsung Galaxy S21", "black", "128", 800, 5),
        Product.CreateNew(ProductId.Of("78dc45cc"),"Xiaomi", "Xiaomi Mi 11", "blue", "256", 700, 2),
    };

    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            var address = Address.CreateNew("1060/2, Kha Vạn Cân", "Thủ Đức", "HCM", "VN", "Lê Xuân Bách", "lxbach1608@gmail.com", "0333284890");

            var order = Order.CreateNew("ORDER-001", CustomerId.Of(new Guid("78dc45ca-a007-4d33-9616-2d8e44735e1a")), address, address, OrderStatus.PAID, PaymentType.VNPAY);

            order.AddOrderLine(ProductId.Of("78dc45ca"), "iPhone 16", "iPhone 16", "pink", "256", 1, 1000);

            return new List<Order> { order };
        }
    }

}
