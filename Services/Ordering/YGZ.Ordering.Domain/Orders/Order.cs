
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders;

public class Order : AggregateRoot<OrderId>, IAuditable, ISoftDelete
{
    public Order(OrderId id) : base(id) { }

    public TenantId? TenantId { get; init; }
    public BranchId? BranchId { get; init; }
    public required UserId CustomerId { get; init; }
    public required Code Code { get; init; }
    public required EPaymentMethod PaymentMethod { get; init; }
    public required EOrderStatus OrderStatus { get; init; }
    public required ShippingAddress ShippingAddress { get; init; }
    private List<OrderItem> _orderItems { get; init; } = new();
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public required decimal TotalAmount { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    public string? UpdatedBy { get; init; } = null;
    public bool IsDeleted { get; init; } = false;
    public DateTime? DeletedAt { get; init; } = null;
    public string? DeletedBy { get; init; } = null;

    public static Order Create(OrderId orderId,
                               UserId customerId,
                               Code code,
                               EPaymentMethod paymentMethod,
                               EOrderStatus orderStatus,
                               ShippingAddress shippingAddress,
                               decimal totalAmount,
                               TenantId? tenantId = null,
                               BranchId? branchId = null)
    {
        var order = new Order(orderId)
        {
            TenantId = tenantId,
            BranchId = branchId,
            CustomerId = customerId,
            Code = code,
            PaymentMethod = paymentMethod,
            OrderStatus = orderStatus,
            ShippingAddress = shippingAddress,
            TotalAmount = totalAmount
        };

        //order.AddDomainEvent(new OrderCreatedDomainEvent(order));

        return order;
    }

    public void AddOrderItem(OrderItem orderItem)
    {
       _orderItems.Add(orderItem);
    }

    //public void RemoveOrderItem(OrderItemId orderItemId)
    //{
    //    var item = _orderItems.FirstOrDefault(x => x.Id == orderItemId);

    //    if (item is not null)
    //    {
    //        _orderItems.Remove(item);
    //    }
    //}

    //public void Confirm()
    //{
    //    if (Status == OrderStatus.PENDING)
    //    {
    //        Status = OrderStatus.CONFIRMED;
    //    }
    //}

    //public void Cancel()
    //{
    //    if (Status == OrderStatus.PENDING)
    //    {
    //        Status = OrderStatus.CANCELLED;
    //    }
    //    else if (Status == OrderStatus.PREPARING)
    //    {
    //        Status = OrderStatus.CANCELLED;
    //    }
    //    else if (Status == OrderStatus.CONFIRMED)
    //    {
    //        Status = OrderStatus.CANCELLED;
    //    }

    //}

    //public void Prepare()
    //{
    //    if (Status == OrderStatus.CONFIRMED || Status == OrderStatus.PAID)
    //    {
    //        Status = OrderStatus.PREPARING;
    //    }
    //}

    //public void Deliver()
    //{
    //    if (Status == OrderStatus.PREPARING)
    //    {
    //        Status = OrderStatus.DELIVERING;
    //    }
    //}

    //public void Delivered()
    //{
    //    if (Status == OrderStatus.DELIVERING)
    //    {
    //        Status = OrderStatus.DELIVERED;
    //    }
    //}
}
