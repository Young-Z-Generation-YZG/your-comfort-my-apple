using Microsoft.AspNetCore.Authorization;

namespace YGZ.BuildingBlocks.Shared.Authorization;

/// <summary>
/// Authorization requirement that enforces X-Tenant-Id header for specific roles.
/// Only users whose roles intersect with <see cref="RolesRequiringTenantHeader"/> must provide the header.
/// </summary>
public class TenantHeaderRequirement : IAuthorizationRequirement
{
    public TenantHeaderRequirement(params string[] rolesRequiringTenantHeader)
    {
        RolesRequiringTenantHeader = new HashSet<string>(rolesRequiringTenantHeader ?? Array.Empty<string>());
    }

    public HashSet<string> RolesRequiringTenantHeader { get; }
}

