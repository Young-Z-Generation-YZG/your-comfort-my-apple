
namespace YGZ.Catalog.Api.Contracts;

public class GetProductsRequest
{
    public int? _page { get; set; } = 1;
    public int? _limit { get; set; } = 10;
    public string? _productColor { get; set; }
    public string? _productStorage { get; set; }
    public string? _priceFrom { get; set; }
    public string? _priceTo { get; set; }
    public string? _priceSort { get; set; }
}