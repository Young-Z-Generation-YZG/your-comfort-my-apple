using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.BuildingBlocks.Shared.Utils;

public static class ConvertGrpcEnumToNormalEnum
{
    public static EDiscountType ConvertToEDiscountType(string EDiscountTypeGrpc)
    {
        switch (EDiscountTypeGrpc)
        {
            case "DiscountTypePercentage":
                return EDiscountType.PERCENTAGE;
            case "DiscountTypeFixedAmount":
                return EDiscountType.FIXED_AMOUNT;
            default:
                return EDiscountType.UNKNOWN;
        }
    }

    public static EProductClassification ConvertToECategoryType(string ECategoryTypeGrpc)
    {
        switch (ECategoryTypeGrpc)
        {
            case "CategoryTypeIphone":
                return EProductClassification.IPHONE;
            case "CategoryTypeIpad":
                return EProductClassification.IPAD;
            case "CategoryTypeMacbook":
                return EProductClassification.MACBOOK;
            case "CategoryTypeWatch":
                return EProductClassification.WATCH;
            case "CategoryTypeHeadphone":
                return EProductClassification.HEADPHONE;
            case "CategoryTypeAccessory":
                return EProductClassification.ACCESSORY;
            default:
                return EProductClassification.UNKNOWN;
        }
    }
}
