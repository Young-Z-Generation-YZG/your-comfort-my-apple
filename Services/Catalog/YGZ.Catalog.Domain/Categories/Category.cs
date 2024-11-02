

using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Categories;
public class Category : AggregateRoot<CategoryId>, IAuditable
{
    public string Name { get; set; }
    [BsonElement("created_at")]
    public DateTime Created_at { get; }

    [BsonElement("updated_at")]
    public DateTime Updated_at { get; set; }


    private Category(CategoryId categoryId, string name, DateTime created_at, DateTime updated_at) : base(categoryId)
    {
        Name = name;
        Created_at = created_at;
        Updated_at = updated_at;
    }



    public static Category Create(string name)
    {
        var vietnamTimezone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimezone);

        return new Category(CategoryId.Create(), name, vietnamTime, vietnamTime);
    }
}
