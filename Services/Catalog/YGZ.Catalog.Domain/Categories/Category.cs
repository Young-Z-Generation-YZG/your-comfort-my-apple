
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions;
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

    [BsonElement("order")]
    public int Order { get; set; }

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

    [BsonElement("deleted_by")]
    public Guid? DeletedBy => null;

    public static Category Create(CategoryId id, string name, string description, int order, CategoryId? parentId)
    {
        return new Category(id)
        {
            Id = id,
            Name = name,
            Description = description,
            Slug = Slug.Create(name),
            Order = order,
            ParentId = parentId
        };
    }
}
