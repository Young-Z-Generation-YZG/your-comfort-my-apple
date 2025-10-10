namespace YGZ.Catalog.Api.Contracts.IphoneRequest;

public sealed record GetIphoneModelsRequest
{
    public int? _page { get; set; } = 1;
    public int? _limit { get; set; } = 10;
    public List<string>? _colors { get; set; } = new List<string>();
    public List<string>? _storages { get; set; } = new List<string>();
    public List<string>? _models { get; set; } = new List<string>();
    public string? _minPrice { get; set; }
    public string? _maxPrice { get; set; }
    public string? _priceSort { get; set; }
}
