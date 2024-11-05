

using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Categories;
public class Category : AggregateRoot<CategoryId>, IAuditable
{
    public string Name { get; set; }
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; }

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; }


    private Category(CategoryId categoryId, string name, DateTime createdAt, DateTime updatedAt) : base(categoryId)
    {
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }



    public static Category Create(string name)
    {
        var vietnamTimezone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimezone);

        return new Category(CategoryId.CreateNew(), name, vietnamTime, vietnamTime);
    }
}
