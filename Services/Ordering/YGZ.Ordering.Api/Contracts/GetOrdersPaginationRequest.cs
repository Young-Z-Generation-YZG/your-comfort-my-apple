namespace YGZ.Ordering.Api.Contracts;

public sealed record GetOrdersPaginationRequest()
{
    public int? _page { get; set; } = 1;
    public int? _limit { get; set; } = 5;
    public string? _orderName { get; set; }
    public string? _orderCode { get; set; }
    public string? _orderStatus { get; set; }
};
