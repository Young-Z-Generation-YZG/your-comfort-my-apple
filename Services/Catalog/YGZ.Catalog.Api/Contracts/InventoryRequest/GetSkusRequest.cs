namespace YGZ.Catalog.Api.Contracts.InventoryRequest;

public sealed record GetSkusRequest
{
    public int? _page { get; init; } = 1;
    public int? _limit { get; init; } = 10;
    public List<string>? _colors { get; init; } = new List<string>();
    public List<string>? _storages { get; init; } = new List<string>();
    public List<string>? _models { get; init; } = new List<string>();
    
    // Dynamic filters for stock
    public int? _stock { get; init; }
    public string? _stockOperator { get; init; } // ">=", ">", "<", "<=", "==", "!=", "in"
    
    // Dynamic filters for sold
    public int? _sold { get; init; }
    public string? _soldOperator { get; init; } // ">=", ">", "<", "<=", "==", "!=", "in"
}
