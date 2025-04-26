
using YGZ.Ordering.Domain.Core.Abstractions;
using YGZ.Ordering.Domain.Core.Enums;
using YGZ.Ordering.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.Events;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders;

public class Order : AggregateRoot<OrderId>, IAuditable<UserId>
{
    //protected Order(OrderId id) : base(id) { }
    private Order() : base(null!) { }

    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public UserId CustomerId { get; set; } = default!;
    public Code Code { get; set; } = default!;
    public OrderStatus Status { get; set; } = OrderStatus.PENDING;
    required public PaymentMethod PaymentMethod { get; set; }
    public Address ShippingAddress { get; set; }
    public decimal SubTotalAmount
    {
        get => OrderItems.Sum(x => x.ProductUnitPrice * x.Quantity);
        set { }
    }
    public decimal DiscountAmount { get; set; } = 0;
    public decimal TotalAmount
    {
        get => SubTotalAmount - DiscountAmount;
        set { }
    }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public UserId? LastModifiedBy { get; set; } = null; // Changed to public setter

    public static Order Create(OrderId orderId,
                               UserId customerId,
                               Code code,
                               OrderStatus status,
                               PaymentMethod paymentMethod,
                               decimal discountAmount,
                               Address ShippingAddress)
    {
        var order = new Order
        {
            Id = orderId,
            CustomerId = customerId,
            Code = code,
            Status = status,
            PaymentMethod = paymentMethod,
            ShippingAddress = ShippingAddress,
            DiscountAmount = discountAmount,
            LastModifiedBy = null
        };

        order.AddDomainEvent(new OrderCreatedDomainEvent(order));

        return order;
    }

    public void AddOrderItem(OrderItem orderItem)
    {
        _orderItems.Add(orderItem);
    }

    public void RemoveOrderItem(OrderItemId orderItemId)
    {
        var item = _orderItems.FirstOrDefault(x => x.Id == orderItemId);

        if (item is not null)
        {
            _orderItems.Remove(item);
        }
    }

    public void Confirm()
    {
        if (Status == OrderStatus.PENDING)
        {
            Status = OrderStatus.CONFIRMED;
        }
    }

    public void Cancel()
    {
        if (Status == OrderStatus.PENDING)
        {
            Status = OrderStatus.CANCELLED;
        }
        else if (Status == OrderStatus.PREPARING)
        {
            Status = OrderStatus.CANCELLED;
        }
        else if (Status == OrderStatus.CONFIRMED)
        {
            Status = OrderStatus.CANCELLED;
        }

    }

    public void Prepare()
    {
        if (Status == OrderStatus.CONFIRMED || Status == OrderStatus.PAID)
        {
            Status = OrderStatus.PREPARING;
        }
    }

    public void Deliver()
    {
        if (Status == OrderStatus.PREPARING)
        {
            Status = OrderStatus.DELIVERING;
        }
    }

    public void Delivered()
    {
        if (Status == OrderStatus.DELIVERING)
        {
            Status = OrderStatus.DELIVERED;
        }
    }
}
