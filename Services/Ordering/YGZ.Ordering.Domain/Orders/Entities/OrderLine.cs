
using YGZ.Ordering.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Domain.Orders.Entities;

public class OrderLine : Entity<OrderLineId>
{
    public OrderId OrderId { get; private set; } = default!;
    public ProductId ProductId { get; private set; } = default!;
    public string ProductModel { get; private set; } = default!;
    public string ProductColor { get; private set; } = default!;
    public int ProductStorage { get; private set; } = default!;
    public string ProductImageUrl { get; private set; } = default!;
    public int Quantity { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;
    public decimal DiscountAmount { get; private set; } = default!;
    public decimal SubTotal { get; private set; } = default!;

    private OrderLine(
        OrderLineId id,
        OrderId orderId,
        ProductId productId,
        string productModel,
        string productColor,
        int productStorage,
        string productImageUrl,
        int quantity,
        decimal price,
        decimal discountAmount,
        decimal subTotal) : base(id)
    {
        OrderId = orderId;
        ProductId = productId;
        ProductModel = productModel;
        ProductColor = productColor;
        ProductStorage = productStorage;
        ProductImageUrl = productImageUrl;
        Quantity = quantity;
        Price = price;
        DiscountAmount = discountAmount;
        SubTotal = subTotal;
    }

    public static OrderLine CreateNew(
        OrderId orderId,
        ProductId productId,
        string productModel,
        string productColor,
        int productStorage,
        string productImageUrl,
        int quantity,
        decimal price,
        decimal discountAmount,
        decimal subTotal)
    {
        return new OrderLine(
            OrderLineId.CreateNew(),
            orderId,
            productId,
            productModel,
            productColor,
            productStorage,
            productImageUrl,
            quantity,
            price,
            discountAmount,
            subTotal);
    }
}
