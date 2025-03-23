

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
    public decimal ProductUnitPrice { get; set; } = default!;
    public string ProductImage { get; set; } = default!;
    public int Quantity { get; set; } = default!;

    public static OrderItem Create(OrderItemId orderItemId,
                                   OrderId orderId,
                                   string productId,
                                   string productModel,
                                   string productColor,
                                   int productStorage,
                                   decimal productUnitPrice,
                                   string productImage,
                                   int quantity)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productId);
        ArgumentException.ThrowIfNullOrWhiteSpace(productModel);
        ArgumentException.ThrowIfNullOrWhiteSpace(productColor);
        ArgumentException.ThrowIfNullOrWhiteSpace(productImage);
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, 1);


        return new OrderItem
        {
            Id = orderItemId,
            OrderId = orderId,
            ProductId = productId,
            ProductModel = productModel,
            ProductColor = productColor,
            ProductStorage = productStorage,
            ProductUnitPrice = productUnitPrice,
            ProductImage = productImage,
            Quantity = quantity
        };
    }
}
