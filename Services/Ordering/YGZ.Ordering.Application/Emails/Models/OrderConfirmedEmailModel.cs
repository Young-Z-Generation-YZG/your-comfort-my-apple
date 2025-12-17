namespace YGZ.Ordering.Application.Emails.Models;

public class OrderConfirmedEmailModel
{
    public string CustomerName { get; set; } = default!;
    public string OrderCode { get; set; } = default!;
    public string OrderId { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItemEmailModel> OrderItems { get; set; } = new();
    public ShippingAddressEmailModel ShippingAddress { get; set; } = default!;
}

public class OrderItemEmailModel
{
    public string ModelName { get; set; } = default!;
    public string ColorName { get; set; } = default!;
    public string StorageName { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotalAmount { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string DisplayImageUrl { get; set; } = default!;
}

public class ShippingAddressEmailModel
{
    public string ContactName { get; set; } = default!;
    public string ContactEmail { get; set; } = default!;
    public string ContactPhoneNumber { get; set; } = default!;
    public string AddressLine { get; set; } = default!;
    public string District { get; set; } = default!;
    public string Province { get; set; } = default!;
    public string Country { get; set; } = default!;
}

