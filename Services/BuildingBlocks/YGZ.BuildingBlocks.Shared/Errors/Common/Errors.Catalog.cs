namespace YGZ.BuildingBlocks.Shared.Errors.Common;

public static partial class Errors
{
    public static class Catalog
    {
        public static Error CannotParseObjectId = Error.BadRequest(code: "Catalog.ObjectIdConvertError", message: "Parsing objectId Error", serviceName: "CatalogService");
    }
}