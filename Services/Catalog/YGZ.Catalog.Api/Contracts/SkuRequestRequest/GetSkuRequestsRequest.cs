namespace YGZ.Catalog.Api.Contracts.SkuRequestRequest;

public sealed record GetSkuRequestsRequest
{
    public int? _page { get; set; } = 1;
    public int? _limit { get; set; } = 10;
    public string? _requestState { get; set; }
    public string? _transferType { get; set; }
    public string? _branchId { get; set; }
}
