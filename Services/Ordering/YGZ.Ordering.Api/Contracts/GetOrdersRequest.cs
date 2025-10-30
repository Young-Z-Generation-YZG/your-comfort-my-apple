namespace YGZ.Ordering.Api.Contracts;

public sealed record GetOrdersRequest
{
    public int? _page { get; init; } = 1;
    public int? _limit { get; init; } = 10;
    public List<string>? _paymentMethod { get; init; } = new List<string>();
    public List<string>? _orderStatus { get; init; } = new List<string>();
    public string? _orderCode { get; init; }
    public string? _customerEmail { get; init; }
    public string? _customerName { get; init; }
    public string? _customerPhoneNumber { get; init; }
}
