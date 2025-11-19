
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Contracts.Products;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;

namespace YGZ.Catalog.Domain.Categories;

[BsonCollection("Categories")]
[BsonIgnoreExtraElements]
public class Category : Entity<CategoryId>, IAuditable, ISoftDelete
{
    public Category(CategoryId id) : base(id) { }

    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("parent_id")]
    public string? ParentId { get; set; }

    [BsonElement("parent_category")]
    public Category? ParentCategory { get; set; }

    [BsonElement("sub_categories")]
    public List<Category>? SubCategories { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("slug")]
    public required Slug Slug { get; set; }

    [BsonElement("order")]
    public int Order { get; set; } = 0;

    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [BsonElement("UpdatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("UpdatedBy")]
    public string? UpdatedBy { get; set; } = null;

    [BsonElement("IsDeleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("DeletedAt")]
    public DateTime? DeletedAt { get; set; } = null;

    [BsonElement("DeletedBy")]
    public string? DeletedBy { get; set; } = null;


    public static Category Create(CategoryId id, string name, string? description, int order, string? parentId, Category? parentCategory)
    {
        return new Category(id)
        {
            Name = name,
            ParentId = parentId,
            ParentCategory = parentCategory,
            Description = description,
            Slug = Slug.Create(name),
            Order = order,
        };
    }

    public void Update(string? name, string? description, int? order, string? parentId, Category? parentCategory, List<Category>? subCategories)
    {
        this.ParentId = parentId ?? this.ParentId;
        this.Name = name ?? this.Name;
        this.Description = description ?? this.Description;
        this.Order = order.HasValue ? order.Value : this.Order;
        this.ParentCategory = parentCategory ?? this.ParentCategory;
        this.SubCategories = subCategories ?? this.SubCategories;

        this.UpdatedAt = DateTime.UtcNow;
        this.UpdatedBy = "System";
    }

    public CategoryResponse ToResponse(List<ProductModelResponse>? productModels = null)
    {
        return new CategoryResponse
        {
            Id = Id.Value!,
            ParentId = ParentId,
            Name = Name,
            Description = Description,
            Order = Order,
            Slug = Slug.Value,
            ParentCategory = ParentCategory?.ToResponse(),
            SubCategories = SubCategories?.Select(sc => sc.ToResponse()).ToList(),
            ProductModels = productModels,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            UpdatedBy = UpdatedBy,
            IsDeleted = IsDeleted,
            DeletedAt = DeletedAt,
            DeletedBy = DeletedBy
        };
    }
}
