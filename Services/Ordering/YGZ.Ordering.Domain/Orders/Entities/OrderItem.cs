

using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Domain.Orders.Entities;

public class OrderItem : Entity<OrderItemId>
{
    // For EF Core migration only
    private OrderItem() : base(null!) { }

    public required string ProductId { get; set; }
    public required string ModelId { get; set; }
    public required string ProductName { get; set; }
    public required string ProductColorName { get; set; }
    public required decimal ProductUnitPrice { get; set; }
    public required string ProductImage { get; set; }
    public required string ProductSlug { get; set; }
    public required int Quantity { get; set; }
    public Promotion? Promotion { get; set; } = null;
    public bool IsReviewed { get; set; } = false;

    // Add this foreign key property
    public OrderId OrderId { get; set; }

    // Navigation property back to Order
    public Order Order { get; set; }

    public static OrderItem Create(OrderItemId orderItemId,
                                   OrderId orderId,
                                   string productId,
                                   string modelId,
                                   string productName,
                                   string productColorName,
                                   decimal productUnitPrice,
                                   string productImage,
                                   string productSlug,
                                   int quantity,
                                   Promotion? promotion)
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
            ModelId = modelId,
            ProductName = productName,
            ProductColorName = productColorName,
            ProductUnitPrice = productUnitPrice,
            ProductImage = productImage,
            ProductSlug = productSlug,
            Quantity = quantity,
            Promotion = promotion
        };
    }
}
