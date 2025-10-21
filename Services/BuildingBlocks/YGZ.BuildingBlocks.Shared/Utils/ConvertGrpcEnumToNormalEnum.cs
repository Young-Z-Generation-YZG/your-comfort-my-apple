using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.BuildingBlocks.Shared.Utils;

public static class ConvertGrpcEnumToNormalEnum
{
    public static EDiscountType ConvertToEDiscountType(string? discountTypeGrpc)
    {
        if (string.IsNullOrWhiteSpace(discountTypeGrpc))
            return EDiscountType.UNKNOWN;

        return discountTypeGrpc switch
        {
            "DiscountTypePercentage" => EDiscountType.PERCENTAGE,
            "DiscountTypeFixedAmount" => EDiscountType.FIXED_AMOUNT,
            _ => EDiscountType.UNKNOWN
        };
    }

    public static EProductClassification ConvertToEProductClassification(string? productClassificationGrpc)
    {
        if (string.IsNullOrWhiteSpace(productClassificationGrpc))
            return EProductClassification.UNKNOWN;

        return productClassificationGrpc switch
        {
            "ProductClassificationIphone" => EProductClassification.IPHONE,
            "ProductClassificationIpad" => EProductClassification.IPAD,
            "ProductClassificationMacbook" => EProductClassification.MACBOOK,
            "ProductClassificationWatch" => EProductClassification.WATCH,
            "ProductClassificationHeadphone" => EProductClassification.HEADPHONE,
            "ProductClassificationAccessory" => EProductClassification.ACCESSORY,
            _ => EProductClassification.UNKNOWN
        };
    }

    public static ETenantType ConvertToETenantType(string? tenantTypeGrpc)
    {
        if (string.IsNullOrWhiteSpace(tenantTypeGrpc))
            return ETenantType.UNKNOWN;

        return tenantTypeGrpc switch
        {
            "TenantTypeWareHouse" => ETenantType.WARE_HOUSE,
            "TenantTypeBranch" => ETenantType.BRANCH,
            _ => ETenantType.UNKNOWN
        };
    }

    public static ETenantState ConvertToETenantState(string? tenantStateGrpc)
    {
        if (string.IsNullOrWhiteSpace(tenantStateGrpc))
            return ETenantState.UNKNOWN;

        return tenantStateGrpc switch
        {
            "TenantStateActive" => ETenantState.ACTIVE,
            "TenantStateInactive" => ETenantState.INACTIVE,
            "TenantStateMaintenance" => ETenantState.MAINTENANCE,
            _ => ETenantState.UNKNOWN
        };
    }
}
