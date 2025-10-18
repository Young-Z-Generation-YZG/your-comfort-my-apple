
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;

namespace YGZ.Catalog.Domain.Categories;

[BsonCollection("Categories")]
public class Category : Entity<CategoryId>, IAuditable, ISoftDelete
{
    public Category(CategoryId id) : base(id)
    {

    }

    [BsonElement("parent_id")]
    public CategoryId? ParentId { get; set; } = null;

    [BsonElement("name")]
    public string Name { get; set; } = default!;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("slug")]
    public Slug Slug { get; set; } = default!;

    [BsonElement("order")]
    public int Order { get; set; }


    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;

    public string? UpdatedBy { get; init; } = null;

    public bool IsDeleted { get; init; } = false;

    public DateTime? DeletedAt { get; init; } = null;

    public string? DeletedBy { get; init; } = null;


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

    public CategoryResponse ToResponse()
    {
        return new CategoryResponse
        {
            Id = Id.Value!,
            Name = Name,
            Description = Description,
            Order = Order,
            Slug = Slug.Value,
            ParentId = ParentId?.Value ?? null,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            UpdatedBy = UpdatedBy,
            IsDeleted = IsDeleted,
            DeletedAt = DeletedAt,
            DeletedBy = DeletedBy
        };
    }
}
