

using MongoDB.Bson;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products;

public class Product : Document, ISoftDelete
{
    public bool IsDeleted => false;
    public DateTime? DeletedAt => null;
    public string? DeletedByUserId => null;
}
