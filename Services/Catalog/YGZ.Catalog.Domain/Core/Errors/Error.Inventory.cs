using YGZ.BuildingBlocks.Shared.Errors;


namespace YGZ.Catalog.Domain.Core.Errors;

public static partial class Errors
{
    public static class Inventory
    {
        
        public static Error SkuDoesNotExist = Error.BadRequest(code: "Inventory.SkuDoesNotExist", message: "Sku does not exist", serviceName: "CatalogService");
        public static Error ReservedQuantityIsLessThanTheQuantityToDeduct = Error.BadRequest(code: "Inventory.ReservedQuantityIsLessThanTheQuantityToDeduct", message: "Reserved quantity is less than the quantity to deduct", serviceName: "CatalogService");
        public static Error QuantityIsLessThanTheQuantityToDeduct = Error.BadRequest(code: "Inventory.QuantityIsLessThanTheQuantityToDeduct", message: "Quantity is less than the quantity to deduct", serviceName: "CatalogService");
        public static Error InsufficientQuantity = Error.BadRequest(code: "Inventory.InsufficientQuantity", message: "Insufficient quantity", serviceName: "CatalogService");
    }
}