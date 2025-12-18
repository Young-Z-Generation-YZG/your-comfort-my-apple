namespace YGZ.Ordering.Api.Contracts;

public sealed record GetOrdersPaginationRequest()
{
    public int? _page { get; set; } = 1;
    public int? _limit { get; set; } = 10;
    public string? _orderCode { get; set; }
    public List<string>? _orderStatus { get; set; } = new List<string>();
    public List<string>? _paymentMethod { get; set; } = new List<string>();
};
