

using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Domain.Orders.Entities;

public class OrderItem : Entity<OrderItemId>
{
    public OrderItem(OrderItemId id) : base(id)
    {
    }

    private OrderItem(OrderItemId id, string ProductId) : base(id)
    {

    }

    // Add this foreign key property
    public OrderId OrderId { get; set; }

    // Optional: Navigation property back to Order
    public Order Order { get; set; }

    public string ProductId { get; set; } = default!;

    public string ProductModel { get; set; } = default!;

    public string ProductColor { get; set; } = default!;

    public string ProductColorHex { get; set; } = default!;

    public int ProductStorage { get; set; } = default!;

    public decimal ProductPrice { get; set; } = default!;

    public string ProductImage { get; set; } = default!;

    public int Quantity { get; set; } = default!;

    public static OrderItem Create(
                                    string ProductId,
                                    string ProductModel,
                                    string ProductColor,
                                    string ProductColorHex,
                                    int ProductStorage,
                                    decimal ProductPrice,
                                    string ProductImage,
                                    int Quantity)
    {
        return new OrderItem(OrderItemId.Create())
        {
            ProductId = ProductId,
            ProductModel = ProductModel,
            ProductColor = ProductColor,
            ProductColorHex = ProductColorHex,
            ProductStorage = ProductStorage,
            ProductPrice = ProductPrice,
            ProductImage = ProductImage,
            Quantity = Quantity
        };
    }
}
