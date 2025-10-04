
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Events;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Domain.Products.Iphone.Entities;

[BsonCollection("Reviews")]
public class Review : Entity<ReviewId>, IAuditable, ISoftDelete
{
    public Review(ReviewId id) : base(id) { }

    [BsonElement("model_id")]
    public ModelId ModelId { get; init; }

    [BsonElement("sku_id")]
    public SKUId SKUId { get; init; }

    [BsonElement("customer_order")]
    public required CustomerOrder CustomerOrder { get; init; }

    [BsonElement("content")]
    public string Content { get; private set; }

    [BsonElement("rating")]
    public int Rating { get; private set; }

    public DateTime CreatedAt => DateTime.Now;

    public DateTime UpdatedAt => DateTime.Now;

    public bool IsDeleted => false;

    public DateTime? DeletedAt => null;

    public string? DeletedBy => null;

    public string? ModifiedBy => null;

    public static Review Create(ModelId modelId, SKUId skuId, CustomerOrder customerOrder, string content, int rating)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);
        ArgumentOutOfRangeException.ThrowIfLessThan(rating, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(rating, 5);

        var review = new Review(ReviewId.Create())
        {
            ModelId = modelId,
            SKUId = skuId,
            CustomerOrder = customerOrder,
            Content = content,
            Rating = rating,
        };

        //review.AddDomainEvent(new ReviewCreatedDomainEvent(review));

        return review;
    }

    public void Update(string newContent, int newRating, Review oldReview)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newContent);
        ArgumentOutOfRangeException.ThrowIfLessThan(newRating, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(newRating, 5);

        Content = newContent;
        Rating = newRating;

        AddDomainEvent(new ReviewUpdatedDomainEvent(oldReview, this));
    }

    public void Delete()
    {
        AddDomainEvent(new ReviewDeletedDomainEvent(this));
    }
}
