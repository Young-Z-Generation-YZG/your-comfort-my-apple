using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;

namespace YGZ.BuildingBlocks.Shared.Implementations.HttpContext;

public class TenantHttpContext : ITenantHttpContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<TenantHttpContext> _logger;
    private const string TenantIdHeaderName = "X-TenantId";

    public TenantHttpContext(IHttpContextAccessor httpContextAccessor, ILogger<TenantHttpContext> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public string GetBranchId()
    {
        var branchId = _httpContextAccessor.HttpContext?.User.FindFirst("branch_id")?.Value ?? _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == "branch_id")?.Value;

        if (string.IsNullOrEmpty(branchId))
        {
            _logger.LogWarning("No branch ID found in user claims.");
            throw new UnauthorizedAccessException("Branch ID not found in token.");
        }

        return branchId;
    }

    public string? GetTenantId()
    {
        var tenantIdFromHeader = _httpContextAccessor.HttpContext?.Request.Headers[TenantIdHeaderName].FirstOrDefault();
        var tenantIdFromUser = _httpContextAccessor.HttpContext?.User.FindFirst("tenant_id")?.Value ?? _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == "tenant_id")?.Value;

        var tenantId = tenantIdFromHeader ?? tenantIdFromUser;

        _logger.LogInformation("Tenant ID: {TenantId}", tenantId);

        // if (string.IsNullOrEmpty(tenantId))
        // {
        //     _logger.LogWarning("No tenant ID found in user claims.");

        //     throw new UnauthorizedAccessException("Tenant ID not found in token.");
        // }

        return tenantId;
    }

    public string GetTenantCode()
    {
        var tenantCode = _httpContextAccessor.HttpContext?.User.FindFirst("tenant_code")?.Value ?? _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == "tenant_code")?.Value;

        if (string.IsNullOrEmpty(tenantCode))
        {
            _logger.LogWarning("No tenant code found in user claims.");
            throw new UnauthorizedAccessException("Tenant code not found in token.");
        }

        return tenantCode;
    }
}
