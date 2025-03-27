namespace YGZ.Catalog.Api.Contracts.IPhoneRequest;

public sealed record GetIPhonePromotionRequest
{
    public int? _page { get; set; } = 1;
    public int? _limit { get; set; } = 9;
    public string? _iphoneModel { get; set; }
}
