namespace YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;

public interface ITenantHttpContext
{
    string GetTenantId();
    string GetTenantCode();
    string GetBranchId();
}
