
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
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
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
    public string? PromotionId { get; init; }
    public string? PromotionType { get; init; }
    public string? DiscountType { get; init; }
    public decimal? DiscountValue { get; init; }
    public decimal? DiscountAmount { get; init; }
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
                               string? promotionId,
                               string? promotionType,
                               string? discountType,
                               decimal? discountValue,
                               decimal? discountAmount,
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
            PromotionId = promotionId,
            PromotionType = promotionType,
            DiscountType = discountType,
            DiscountValue = discountValue,
            DiscountAmount = discountAmount,
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

    public OrderDetailsResponse ToOrderDetailsResponse()
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
            ShippingAddress = new ShippingAddressResponse
            {
                ContactName = ShippingAddress.ContactName,
                ContactEmail = ShippingAddress.ContactEmail,
                ContactPhoneNumber = ShippingAddress.ContactPhoneNumber,
                ContactAddressLine = ShippingAddress.AddressLine,
                ContactDistrict = ShippingAddress.District,
                ContactProvince = ShippingAddress.Province,
                ContactCountry = ShippingAddress.Country
            },
            OrderItems = OrderItems.Select(item => new OrderItemResponse
            {
                OrderId = Id.Value.ToString(),
                OrderItemId = item.Id.Value.ToString(),
                SkuId = item.SkuId?.Value.ToString() ?? null,
                ModelId = item.ModelId,
                ModelName = item.ModelName,
                ColorName = item.ColorName,
                StorageName = item.StorageName,
                UnitPrice = item.UnitPrice,
                DisplayImageUrl = item.DisplayImageUrl,
                ModelSlug = item.ModelSlug,
                Quantity = item.Quantity,
                Promotion = !string.IsNullOrEmpty(item.PromotionId) ? new PromotionResponse
                {
                    PromotionId = item.PromotionId,
                    PromotionType = item.PromotionType ?? string.Empty,
                    DiscountType = item.DiscountType ?? string.Empty,
                    DiscountValue = item.DiscountValue ?? 0,
                    DiscountAmount = item.DiscountAmount ?? 0,
                    FinalPrice = item.DiscountAmount.HasValue 
                        ? item.UnitPrice - item.DiscountAmount.Value 
                        : item.UnitPrice
                } : null,
                IsReviewed = item.IsReviewed,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                UpdatedBy = item.UpdatedBy,
                IsDeleted = item.IsDeleted,
                DeletedAt = item.DeletedAt,
                DeletedBy = item.DeletedBy
            }).ToList(),
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
