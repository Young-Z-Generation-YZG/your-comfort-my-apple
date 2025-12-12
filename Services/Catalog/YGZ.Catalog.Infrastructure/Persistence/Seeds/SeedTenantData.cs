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
                Tenant.Create(TenantId.Of("664355f845e56534956be32b"), "Ware house", "admin", ETenantType.WAREHOUSE, Branch.Create(BranchId.Of("664357a235e84033bbd0e6b6"), TenantId.Of("664355f845e56534956be32b"), "Ware house branch", "Ware house address", null)),
                Tenant.Create(TenantId.Of("690e034dff79797b05b3bc89"), "HCM TD KVC 1060", "hcm-td-kvc-1060", ETenantType.BRANCH, Branch.Create(BranchId.Of("690e034dff79797b05b3bc88"), TenantId.Of("690e034dff79797b05b3bc89"), "HCM_TD_KVC_1060", "số 1060, Kha Vạn Cân, phường Linh Chiểu, Thủ Đức", null)),
                Tenant.Create(TenantId.Of("690e034dff79797b05b3bc90"), "HCM Q1 CMT8 92", "hcm-q1-cmt8-92", ETenantType.BRANCH, Branch.Create(BranchId.Of("690e034dff79797b05b3bc12"), TenantId.Of("690e034dff79797b05b3bc90"), "HCM_Q1_CMT8_92", "số 92, Cách Mạng Tháng 8, Quận 1, Quận 1", null)),
                Tenant.Create(TenantId.Of("690e034dff79797b05b3bc91"), "HCM Q9 LVV 33", "hcm-q9-lvv-33", ETenantType.BRANCH, Branch.Create(BranchId.Of("690e034dff79797b05b3bc13"), TenantId.Of("690e034dff79797b05b3bc91"), "HCM_Q9_LVV_33", "số 33, Lê Văn Việt, phường Tăng Nhơn Phú A, Quận 9", null))
            };
        }
    }
}
