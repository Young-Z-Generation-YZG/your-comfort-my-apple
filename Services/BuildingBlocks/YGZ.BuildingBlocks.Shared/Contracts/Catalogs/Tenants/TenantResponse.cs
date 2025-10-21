namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Tenants;

public sealed record TenantResponse
{
    public required string Id { get; init; }
    public required string Code { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string TenantType { get; init; }
    public required string TenantState { get; init; }
    public DateTime? CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }
    public string? DeletedBy { get; init; }

}
