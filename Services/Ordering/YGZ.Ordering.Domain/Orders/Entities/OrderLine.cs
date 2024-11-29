
using YGZ.Ordering.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Domain.Orders.Entities;

public class OrderLine : Entity<OrderLineId>
{
    public OrderId OrderId { get; private set; } = default!;
    public ProductId ProductId { get; private set; } = default!;
    public string ProductName { get; private set; } = default!;
    public string ProductModel { get; private set; } = default!;
    public string ProductColor { get; private set; } = default!;
    public string ProductStorage { get; private set; } = default!;
    public int Quantity { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;

    private OrderLine(
        OrderLineId id,
        OrderId orderId,
        ProductId productId,
        string productName,
        string productModel,
        string productColor,
        string productStorage,
        int quantity,
        decimal price) : base(id)
    {
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName;
        ProductModel = productModel;
        ProductColor = productColor;
        ProductStorage = productStorage;
        Quantity = quantity;
        Price = price;
    }

    public static OrderLine CreateNew(
        OrderId orderId,
        ProductId productId,
        string productName,
        string productModel,
        string productColor,
        string productStorage,
        int quantity,
        decimal price)
    {
        return new OrderLine(
            OrderLineId.CreateNew(),
            orderId,
            productId,
            productName,
            productModel,
            productColor,
            productStorage,
            quantity,
            price);
    }
}
