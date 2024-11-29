
using YGZ.Ordering.Domain.Core.Abstractions;
using YGZ.Ordering.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.Events;
using YGZ.Ordering.Domain.Orders.ValueObjects;
using static YGZ.Ordering.Domain.Core.Enums.Enums;

namespace YGZ.Ordering.Domain.Orders;

public sealed class Order : AggregateRoot<OrderId>, IAuditable
{
    private readonly List<OrderLine> _orderLines = new();
    public IReadOnlyList<OrderLine> OrderLines => _orderLines.AsReadOnly();

    public string OrderName { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public Address ShippingAddress { get; private set; }
    public Address BillingAddress { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.PENDING;
    public PaymentType PaymentType { get; private set; }
    public decimal TotalAmount
    {
        get => OrderLines.Sum(x => x.Price * x.Quantity);
        private set { }
    }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    private Order(OrderId id,
                  string orderName,
                  CustomerId customerId,
                  Address shippingAddress,
                  Address billingAddress,
                  OrderStatus orderStatus,
                  PaymentType paymentType) : base(id)
    {
        OrderName = orderName;
        CustomerId = customerId;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Status = orderStatus;
        PaymentType = paymentType;
    }

    public static Order CreateNew(
        OrderId id,
        string orderName,
        CustomerId customerId,
        Address shippingAddress,
        Address billingAddress,
        OrderStatus orderStatus,
        PaymentType paymentType)
    {
        var newOrder = new Order(
                        id,
                        orderName,
                        customerId,
                        shippingAddress,
                        billingAddress,
                        orderStatus,
                        paymentType
        );

        newOrder.AddDomainEvent(new OrderCreatedDomainEvent(newOrder));

        return newOrder;
    }

    public void Update(string orderName, Address shippingAddress, Address billingAddress, OrderStatus orderStatus, PaymentType paymentType)
    {
        OrderName = orderName;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Status = orderStatus;
        PaymentType = paymentType;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new OrderUpdatedDomainEvent(this));
    }

    public void AddOrderLine(string productId, string productName, string productModel, string productColor, string productStorage, int quantity, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var newOrderLine = OrderLine.CreateNew(
            Id,
            productId,
            productName,
            productModel,
            productColor,
            productStorage,
            quantity,
            price);

        _orderLines.Add(newOrderLine);
    }

    public void RemoveOrderLine(OrderLineId orderLineId)
    {
        var orderLine = _orderLines.FirstOrDefault(x => x.Id == orderLineId);
        if (orderLine is not null)
        {
            _orderLines.Remove(orderLine);
        }
    }
}
