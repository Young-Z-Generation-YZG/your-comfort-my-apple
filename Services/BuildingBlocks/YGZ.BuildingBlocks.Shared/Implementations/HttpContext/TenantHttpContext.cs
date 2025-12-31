using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;

namespace YGZ.BuildingBlocks.Shared.Implementations.HttpContext;

public class TenantHttpContext : ITenantHttpContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<TenantHttpContext> _logger;
    private const string TenantIdHeaderName = "X-TenantId";
    private const string TenantIdClaimName = "tenant_id";
    private const string BranchIdClaimName = "branch_id";
    private const string SubDomainClaimName = "sub_domain";

    public TenantHttpContext(IHttpContextAccessor httpContextAccessor, ILogger<TenantHttpContext> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public string? GetBranchId()
    {
        var branchId = _httpContextAccessor.HttpContext?.User.FindFirst(BranchIdClaimName)?.Value ?? _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == BranchIdClaimName)?.Value;

        if (string.IsNullOrEmpty(branchId))
        {
            _logger.LogWarning("No branch ID found in user claims.");
        }

        return branchId;
    }

    public string? GetTenantId()
    {
        var tenantIdFromHeader = _httpContextAccessor.HttpContext?.Request.Headers[TenantIdHeaderName].FirstOrDefault();
        var tenantIdFromUser = _httpContextAccessor.HttpContext?.User.FindFirst(TenantIdClaimName)?.Value ?? _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == TenantIdClaimName)?.Value;

        var tenantId = tenantIdFromHeader ?? tenantIdFromUser;

        _logger.LogInformation("Tenant ID: {TenantId}", tenantId);

        if (string.IsNullOrEmpty(tenantId))
        {
            _logger.LogWarning("No tenant ID found in user claims.");
        }

        return tenantId;
    }

    public string GetSubDomain()
    {
        var subDomain = _httpContextAccessor.HttpContext?.User.FindFirst(SubDomainClaimName)?.Value ?? _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == SubDomainClaimName)?.Value;

        if (string.IsNullOrEmpty(subDomain))
        {
            _logger.LogWarning("No subdomain found in user claims.");
            throw new UnauthorizedAccessException("Subdomain not found in token.");
        }

        return subDomain;
    }
}
