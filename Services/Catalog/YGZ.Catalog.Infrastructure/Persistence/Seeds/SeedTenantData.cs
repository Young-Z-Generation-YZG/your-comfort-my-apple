using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Domain.Tenants;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Seeds;

public static class SeedTenantData
{
    public static IEnumerable<Tenant> Tenants
    {
        get
        {
            return new List<Tenant>
            {
                Tenant.Create(TenantId.Of("664355f845e56534956be32b"), "Ware house", ETenantType.WARE_HOUSE, Branch.Create(BranchId.Of("664357a235e84033bbd0e6b6"), TenantId.Of("664355f845e56534956be32b"), "Ware house branch", "Ware house address", null))
            };
        }
    }

    public static IEnumerable<Branch> Branches
    {
        get
        {
            return new List<Branch>
            {
                Branch.Create(BranchId.Of("664357a235e84033bbd0e6b6"), TenantId.Of("664355f845e56534956be32b"), "", "Ware house address", null)
            };
        }
    }
}
