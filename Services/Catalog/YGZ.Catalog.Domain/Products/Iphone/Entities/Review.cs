
using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Contracts.Reviews;
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
    public required ModelId ModelId { get; init; }

    [BsonElement("sku_id")]
    public required SkuId SkuId { get; init; }

    [BsonElement("order_info")]
    public required OrderInfo OrderInfo { get; init; }

    [BsonElement("customer_review_info")]
    public required CustomerReviewInfo CustomerReviewInfo { get; init; }

    [BsonElement("content")]
    public required string Content { get; set; }

    [BsonElement("rating")]
    public required int Rating { get; set; }

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

    public static Review Create(ReviewId reviewId, ModelId modelId, SkuId skuId, OrderInfo orderInfo, CustomerReviewInfo customerReviewInfo, string content, int rating)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);
        ArgumentOutOfRangeException.ThrowIfLessThan(rating, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(rating, 5);

        var review = new Review(reviewId)
        {
            ModelId = modelId,
            SkuId = skuId,
            OrderInfo = orderInfo,
            CustomerReviewInfo = customerReviewInfo,
            Content = content,
            Rating = rating,
        };

        review.AddDomainEvent(new ReviewCreatedDomainEvent(review));

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

    public ProductModelReviewResponse ToProductModelReviewResponse()
    {
        return new ProductModelReviewResponse
        {
            Id = Id.Value!,
            ModelId = ModelId.Value!,
            SkuId = SkuId.Value!,
            OrderInfo = OrderInfo.ToResponse(),
            CustomerReviewInfo = CustomerReviewInfo.ToResponse(),
            Rating = Rating,
            Content = Content,
            CreatedAt = CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            UpdatedAt = UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            UpdatedBy = UpdatedBy,
            IsDeleted = IsDeleted,
            DeletedAt = DeletedAt,
            DeletedBy = DeletedBy
        };
    }

    public ReviewInOrderResponse ToReviewInOrderResponse()
    {
        return new ReviewInOrderResponse
        {
            Id = Id.Value!,
            ModelId = ModelId.Value!,
            SkuId = SkuId.Value!,
            OrderInfo = OrderInfo.ToResponse(),
            Rating = Rating,
            Content = Content,
            CreatedAt = CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            UpdatedAt = UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            UpdatedBy = UpdatedBy,
            IsDeleted = IsDeleted,
            DeletedAt = DeletedAt,
            DeletedBy = DeletedBy
        };
    }
}
