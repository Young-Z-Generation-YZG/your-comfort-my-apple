using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.BuildingBlocks.Shared.Utils;

public static class ConvertGrpcEnumToNormalEnum
{
    public static EDiscountType ConvertToEDiscountType(string discountTypeGrpc)
    {
        return discountTypeGrpc switch
        {
            DiscountGrpcEnumConstants.DISCOUNT_TYPE_PERCENTAGE_GRPC_ENUM => EDiscountType.PERCENTAGE,
            DiscountGrpcEnumConstants.DISCOUNT_TYPE_FIXED_AMOUNT_GRPC_ENUM => EDiscountType.FIXED_AMOUNT,
            _ => EDiscountType.UNKNOWN
        };
    }

    public static EProductClassification ConvertToEProductClassification(string productClassificationGrpc)
    {
        return productClassificationGrpc switch
        {
            DiscountGrpcEnumConstants.PRODUCT_CLASSIFICATION_IPHONE_GRPC_ENUM => EProductClassification.IPHONE,
            DiscountGrpcEnumConstants.PRODUCT_CLASSIFICATION_IPAD_GRPC_ENUM => EProductClassification.IPAD,
            DiscountGrpcEnumConstants.PRODUCT_CLASSIFICATION_MACBOOK_GRPC_ENUM => EProductClassification.MACBOOK,
            DiscountGrpcEnumConstants.PRODUCT_CLASSIFICATION_WATCH_GRPC_ENUM => EProductClassification.WATCH,
            DiscountGrpcEnumConstants.PRODUCT_CLASSIFICATION_HEADPHONE_GRPC_ENUM => EProductClassification.HEADPHONE,
            DiscountGrpcEnumConstants.PRODUCT_CLASSIFICATION_ACCESSORY_GRPC_ENUM => EProductClassification.ACCESSORY,
            _ => EProductClassification.UNKNOWN
        };
    }

    public static ETenantType ConvertToETenantType(string? tenantTypeGrpc)
    {
        if (string.IsNullOrWhiteSpace(tenantTypeGrpc))
            return ETenantType.UNKNOWN;

        return tenantTypeGrpc switch
        {
            "TenantTypeWareHouse" => ETenantType.WAREHOUSE,
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
