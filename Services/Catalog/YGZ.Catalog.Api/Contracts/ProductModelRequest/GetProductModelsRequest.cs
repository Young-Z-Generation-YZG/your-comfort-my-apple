namespace YGZ.Catalog.Api.Contracts.ProductModelRequest;

public sealed record GetProductModelsRequest
{
    public int? _page { get; set; } = 1;
    public int? _limit { get; set; } = 10;
    public string? _textSearch { get; set; }
}
