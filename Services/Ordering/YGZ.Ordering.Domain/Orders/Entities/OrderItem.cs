

using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Domain.Orders.Entities;

public class OrderItem : Entity<OrderItemId>
{
    // For EF Core migration only
    private OrderItem() : base(null!) { }

    // Add this foreign key property
    public OrderId OrderId { get; set; }

    // Navigation property back to Order
    public Order Order { get; set; }
    public string ProductId { get; set; } = default!;
    public string ProductModel { get; set; } = default!;
    public string ProductColor { get; set; } = default!;
    public string ProductColorHex { get; set; } = default!;
    public int ProductStorage { get; set; } = default!;
    public decimal ProductPrice { get; set; } = default!;
    public string ProductImage { get; set; } = default!;
    public int Quantity { get; set; } = default!;

    public static OrderItem Create(OrderItemId orderItemId,
                                   OrderId orderId,
                                   string ProductId,
                                   string ProductModel,
                                   string ProductColor,
                                   string ProductColorHex,
                                   int ProductStorage,
                                   decimal ProductPrice,
                                   string ProductImage,
                                   int Quantity)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ProductId);
        ArgumentException.ThrowIfNullOrWhiteSpace(ProductModel);
        ArgumentException.ThrowIfNullOrWhiteSpace(ProductColor);
        ArgumentException.ThrowIfNullOrWhiteSpace(ProductColorHex);
        ArgumentException.ThrowIfNullOrWhiteSpace(ProductImage);
        ArgumentOutOfRangeException.ThrowIfLessThan(Quantity, 1);


        return new OrderItem
        {
            Id = orderItemId,
            OrderId = orderId,
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
