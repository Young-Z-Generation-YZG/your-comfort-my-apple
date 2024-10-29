
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Domain.Products;

public sealed class Product : AggregateRoot<ProductId>, IAuditable
{
    public ProductId ProductId { get; }

    [BsonElement("name")]
    public string Name { get; }

    [BsonElement("image_urls")]
    public string[] Image_urls { get; }

    [BsonElement("image_ids")]
    public string[] Image_ids { get; }

    [BsonElement("slug")]
    public Slug Slug { get; }

    [BsonDateTimeOptions]
    [BsonElement("created_at")]
    public DateTime Created_at { get;}

    [BsonDateTimeOptions]
    [BsonElement("updated_at")]
    public DateTime Updated_at { get ; set; }


    private Product(ProductId productId, string name, string[] image_urls, string[] image_ids, Slug slug, DateTime created_at, DateTime updated_at) : base(productId)
    {
        ProductId = productId;
        Name = name;
        Image_urls = image_urls;
        Image_ids = image_ids;
        Slug = slug;
        Created_at = created_at;
        Updated_at = updated_at;
    }

    public static Product Create(string name, string[]? image_urls= null, string[]? image_ids = null)
    {
        return new Product(ProductId.CreateUnique(), name, image_urls ?? [], image_ids ?? [], Slug.Create(name), DateTime.UtcNow, DateTime.UtcNow);
    }
}
