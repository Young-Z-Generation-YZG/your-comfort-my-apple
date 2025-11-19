
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.Events;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders;

public class Order : AggregateRoot<OrderId>, IAuditable, ISoftDelete
{
    public Order(OrderId id) : base(id) { }

    public TenantId? TenantId { get; init; }
    public BranchId? BranchId { get; init; }
    public required UserId CustomerId { get; init; }
    public string? CustomerPublicKey { get; init; }
    public string? Tx { get; init; }
    public required Code Code { get; init; }
    public required EPaymentMethod PaymentMethod { get; init; }
    public EOrderStatus OrderStatus { get; private set; } = EOrderStatus.UNKNOWN;
    public required ShippingAddress ShippingAddress { get; init; }
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
    public string? PromotionId { get; init; }
    public string? PromotionType { get; init; }
    public string? DiscountType { get; init; }
    public decimal? DiscountValue { get; init; }
    public decimal? DiscountAmount { get; init; }
    public decimal TotalAmount { get; set; } = 0;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; } = null;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; set; } = null;

    public static Order Create(OrderId orderId,
                               UserId customerId,
                               string? customerPublicKey,
                               string? tx,
                               Code code,
                               EPaymentMethod paymentMethod,
                               EOrderStatus orderStatus,
                               ShippingAddress shippingAddress,
                               string? promotionId,
                               string? promotionType,
                               string? discountType,
                               decimal? discountValue,
                               decimal? discountAmount,
                               decimal? totalAmount = 0,
                               TenantId? tenantId = null,
                               BranchId? branchId = null,
                               DateTime? createdAt = null)
    {
        var order = new Order(orderId)
        {
            TenantId = tenantId,
            BranchId = branchId,
            CustomerId = customerId,
            CustomerPublicKey = customerPublicKey,
            Tx = tx,
            Code = code,
            PaymentMethod = paymentMethod,
            OrderStatus = orderStatus,
            ShippingAddress = shippingAddress,
            PromotionId = promotionId,
            PromotionType = promotionType,
            DiscountType = discountType,
            DiscountValue = discountValue,
            DiscountAmount = discountAmount,
            TotalAmount = totalAmount ?? 0,
            CreatedAt = createdAt ??= DateTime.UtcNow
        };

        //order.AddDomainEvent(new OrderCreatedDomainEvent(order));

        return order;
    }

    public void AddOrderItem(OrderItem orderItem)
    {
        _orderItems.Add(orderItem);
    }

    public void SetPaid()
    {
        if (OrderStatus != EOrderStatus.PENDING)
        {
            throw new InvalidOperationException($"Order is not in status {EOrderStatus.PENDING.Name}");
        }

        OrderStatus = EOrderStatus.PAID;

        this.AddDomainEvent(new OrderPaidDomainEvent(this));
    }

    public void SetConfirmed()
    {
        if (OrderStatus != EOrderStatus.PENDING)
        {
            throw new InvalidOperationException($"Order is not in status {EOrderStatus.PENDING.Name}");
        }
        OrderStatus = EOrderStatus.CONFIRMED;

        this.AddDomainEvent(new OrderConfirmedDomainEvent(this));
    }

    public void SetCancelled()
    {
        if (OrderStatus != EOrderStatus.PENDING)
        {
            throw new InvalidOperationException($"Order is not in status {EOrderStatus.PENDING.Name}");
        }

        OrderStatus = EOrderStatus.CANCELLED;
    }

    public void SetPreparing()
    {
        if (OrderStatus != EOrderStatus.CONFIRMED)
        {
            throw new InvalidOperationException($"Order is not in status {EOrderStatus.CONFIRMED.Name}");
        }

        OrderStatus = EOrderStatus.PREPARING;
    }
    //{
    public void SetDelivering()
    {
        if (OrderStatus != EOrderStatus.PREPARING)
        {
            throw new InvalidOperationException($"Order is not in status {EOrderStatus.PREPARING.Name}");
        }
    }

    public void SetDelivered()
    {
        if (OrderStatus != EOrderStatus.DELIVERING)
        {
            throw new InvalidOperationException($"Order is not in status {EOrderStatus.DELIVERING.Name}");
        }

        this.AddDomainEvent(new OrderDeliveredDomainEvent(this));
    }

    public OrderDetailsResponse ToResponse()
    {
        return new OrderDetailsResponse
        {
            TenantId = TenantId?.Value.ToString() ?? null,
            BranchId = BranchId?.Value.ToString() ?? null,
            OrderId = Id.Value.ToString(),
            CustomerId = CustomerId.Value.ToString(),
            CustomerEmail = ShippingAddress.ContactEmail,
            OrderCode = Code.Value,
            Status = OrderStatus.Name,
            PaymentMethod = PaymentMethod.Name,
            ShippingAddress = ShippingAddress.ToResponse(),
            OrderItems = OrderItems.Select(item => item.ToResponse()).ToList(),
            PromotionId = PromotionId,
            PromotionType = PromotionType,
            DiscountType = DiscountType,
            DiscountValue = DiscountValue,
            DiscountAmount = DiscountAmount,
            TotalAmount = TotalAmount,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            UpdatedBy = UpdatedBy,
            IsDeleted = IsDeleted,
            DeletedAt = DeletedAt,
            DeletedBy = DeletedBy
        };
    }
}
