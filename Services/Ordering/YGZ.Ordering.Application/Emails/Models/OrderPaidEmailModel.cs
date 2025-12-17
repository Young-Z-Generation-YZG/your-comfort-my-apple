namespace YGZ.Ordering.Application.Emails.Models;

public class OrderPaidEmailModel
{
    public string CustomerName { get; set; } = default!;
    public string OrderCode { get; set; } = default!;
    public string OrderId { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItemEmailModel> OrderItems { get; set; } = new();
    public ShippingAddressEmailModel ShippingAddress { get; set; } = default!;
}

