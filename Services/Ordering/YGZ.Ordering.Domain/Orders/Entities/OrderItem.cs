

using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Domain.Orders.Entities;

public class OrderItem : Entity<OrderItemId>
{
    // For EF Core migration only
    private OrderItem() : base(null!) { }

    public required string ProductId { get; set; } = default!;
    public required string ProductName { get; set; } = default!;
    public required string ProductColorName { get; set; } = default!;
    public required decimal ProductUnitPrice { get; set; } = default!;
    public required string ProductImage { get; set; } = default!;
    public required string ProductSlug { get; set; } = default!;
    public required int Quantity { get; set; } = default!;



    // Add this foreign key property
    public OrderId OrderId { get; set; }

    // Navigation property back to Order
    public Order Order { get; set; }

    public static OrderItem Create(OrderItemId orderItemId,
                                   OrderId orderId,
                                   string productId,
                                   string productName,
                                   string productColorName,
                                   decimal productUnitPrice,
                                   string productImage,
                                   string productSlug,
                                   int quantity)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productId);
        ArgumentException.ThrowIfNullOrWhiteSpace(productName);
        ArgumentException.ThrowIfNullOrWhiteSpace(productColorName);
        ArgumentException.ThrowIfNullOrWhiteSpace(productImage);
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, 1);


        return new OrderItem
        {
            Id = orderItemId,
            OrderId = orderId,
            ProductId = productId,
            ProductName = productName,
            ProductColorName = productColorName,
            ProductUnitPrice = productUnitPrice,
            ProductImage = productImage,
            ProductSlug = productSlug,
            Quantity = quantity
        };
    }
}
