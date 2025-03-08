

using MongoDB.Bson;

namespace YGZ.Catalog.Application.Common.Validators;

public static partial class SchemaValidators
{
    public static class ObjectIdValidator
    {
        public static bool IsValid(string objectId)
        {
            return ObjectId.TryParse(objectId, out _);
        }
    }
}
