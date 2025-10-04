using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Tenants.Commands;

public sealed record CreateTenantCommand : ICommand<bool>
{
    public required string Name { get; init; }
    public required string BranchAddress { get; init; }
    public string? TenantDescription { get; init; }
    public string? BranchDescription { get; init; }
}
