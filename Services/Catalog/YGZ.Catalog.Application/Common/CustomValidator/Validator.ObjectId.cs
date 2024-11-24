

using MongoDB.Bson;

namespace YGZ.Catalog.Application.Common.CustomValidator;

public static partial class Validators
{
    public static class ObjectIdVlidator
    {
        public static bool BeAValidObjectId(string productId)
        {
            return ObjectId.TryParse(productId, out _);
        }
    }
}
