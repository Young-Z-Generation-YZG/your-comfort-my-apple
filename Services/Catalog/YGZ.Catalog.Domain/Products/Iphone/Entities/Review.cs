
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
    public SkuId SkuId { get; init; }

    [BsonElement("customer_order")]
    public required CustomerOrder CustomerOrder { get; init; }

    [BsonElement("content")]
    public string Content { get; private set; }

    [BsonElement("rating")]
    public int Rating { get; private set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;

    public bool IsDeleted { get; init; } = false;

    public DateTime? DeletedAt { get; init; } = null;

    public string? DeletedBy { get; init; } = null;

    public string? UpdatedBy { get; init; } = null;

    public static Review Create(ModelId modelId, SkuId skuId, CustomerOrder customerOrder, string content, int rating)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);
        ArgumentOutOfRangeException.ThrowIfLessThan(rating, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(rating, 5);

        var review = new Review(ReviewId.Create())
        {
            ModelId = modelId,
            SkuId = skuId,
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
