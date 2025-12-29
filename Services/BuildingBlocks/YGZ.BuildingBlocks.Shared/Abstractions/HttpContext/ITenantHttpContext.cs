namespace YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;

public interface ITenantHttpContext
{
    string? GetTenantId();
    string? GetSubDomain();
    string? GetBranchId();
}
