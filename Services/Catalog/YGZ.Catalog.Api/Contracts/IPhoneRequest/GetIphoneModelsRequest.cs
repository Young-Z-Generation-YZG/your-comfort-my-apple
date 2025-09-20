namespace YGZ.Catalog.Api.Contracts.IphoneRequest;

public sealed record GetIphoneModelsRequest
{
    public int? _page { get; set; } = 1;
    public int? _limit { get; set; } = 10;
    public List<string>? _productColors { get; set; } = new List<string>();
    public List<string>? _productStorages { get; set; } = new List<string>();
    public List<string>? _productModels { get; set; } = new List<string>();
    public string? _priceFrom { get; set; }
    public string? _priceTo { get; set; }
    public string? _priceSort { get; set; }
}
