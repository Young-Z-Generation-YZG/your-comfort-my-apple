
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Categories;

[BsonCollection("Categories")]
public class Category : Entity<CategoryId>, IAuditable, ISoftDelete
{
    public Category(CategoryId id) : base(id)
    {

    }

    [BsonElement("name")]
    public string Name { get; set; } = default!;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("slug")]
    public Slug Slug { get; set; } = default!;

    [BsonElement("parent_id")]
    public CategoryId? ParentId { get; set; } = null;

    [BsonElement("created_at")]
    public DateTime CreatedAt => Id?.Id!.Value.CreationTime ?? DateTime.Now;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt => Id?.Id!.Value.CreationTime ?? DateTime.Now;

    [BsonElement("is_deleted")]
    public bool IsDeleted => false;

    [BsonElement("deleted_at")]
    public DateTime? DeletedAt => null;

    [BsonElement("deleted_by_user_id")]
    public ObjectId? DeletedByUserId => null;

    public static Category Create(string name, string description, string? parentId)
    {
        return new Category(CategoryId.Create())
        {
            Name = name,
            Description = description,
            Slug = Slug.Create(name),
            ParentId = CategoryId.ToId(parentId!)
        };
    }
}
