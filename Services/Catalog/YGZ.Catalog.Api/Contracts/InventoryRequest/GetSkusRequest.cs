namespace YGZ.Catalog.Api.Contracts.InventoryRequest;

public sealed record GetSkusRequest
{
    public int? _page { get; init; } = 1;
    public int? _limit { get; init; } = 10;
    public List<string>? _colors { get; init; } = new List<string>();
    public List<string>? _storages { get; init; } = new List<string>();
    public List<string>? _models { get; init; } = new List<string>();
}
