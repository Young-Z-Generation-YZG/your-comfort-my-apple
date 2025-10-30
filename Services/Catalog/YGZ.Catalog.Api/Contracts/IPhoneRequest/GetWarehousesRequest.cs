using System.Diagnostics.CodeAnalysis;
namespace YGZ.Catalog.Api.Contracts.IphoneRequest;

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Underscore prefix is used for query parameters matching API conventions")]
public sealed record GetWarehousesRequest
{
    public int? _page { get; init; } = 1;
    public int? _limit { get; init; } = 10;
    public List<string>? _colors { get; init; } = new List<string>();
    public List<string>? _storages { get; init; } = new List<string>();
    public List<string>? _models { get; init; } = new List<string>();
}
