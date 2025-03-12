
using YGZ.Ordering.Domain.Core.Abstractions;
using YGZ.Ordering.Domain.Core.Enums;
using YGZ.Ordering.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders;

public class Order : AggregateRoot<OrderId>, IAuditable
{
    public Order(OrderId id, Code code) : base(id)
    {
        Code = code;
    }

    private Order(OrderId id) : base(id)
    {

    }

    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public UserId CustomerId { get; set; } = default!;
    public Code Code { get; set; } = default!;
    public OrderStatusEnum Status { get; set; } = OrderStatusEnum.PENDING;
    required public PaymentTypeEnum PaymentType { get; set; }
    public AddressId ShippingAddressId { get; set; }
    public decimal SubTotal
    {
        get => OrderItems.Sum(x => x.ProductPrice * x.Quantity);
        set { }
    }
    public decimal DiscountAmount { get; set; } = 0;
    public decimal Total
    {
        get => SubTotal - DiscountAmount;
        set { }
    }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
