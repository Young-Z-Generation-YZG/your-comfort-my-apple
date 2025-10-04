namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;

public sealed record BranchWithSkusResponse
{
    required public BranchResponse Branch { get; init; }
    required public List<SkuResponse> Skus { get; init; }
}
