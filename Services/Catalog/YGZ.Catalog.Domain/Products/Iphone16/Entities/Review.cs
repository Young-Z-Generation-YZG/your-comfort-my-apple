
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Iphone16.Events;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Domain.Products.Iphone16.Entities;

[BsonCollection("Reviews")]
public class Review : Entity<ReviewId>, IAuditable
{
    public Review(ReviewId id) : base(id)
    {

    }

    [BsonElement("product_id")]
    required public IPhone16Id ProductId { get; set; } = default!;

    [BsonElement("model_id")]
    required public IPhone16ModelId ModelId { get; set; } = default!;

    [BsonElement("content")]
    required public string Content { get; set; }

    [BsonElement("rating")]
    required public int Rating { get; set; }

    [BsonElement("order_id")]
    required public string OrderId { get; set; }

    [BsonElement("order_item_id")]
    required public string OrderItemId { get; set; }

    [BsonElement("customer_id")]
    required public string CustomerId { get; set; }

    [BsonElement("customer_username")]
    required public string CustomerUserName { get; set; }
    
    [BsonElement("created_at")]
    public DateTime CreatedAt => Id.Id?.CreationTime ?? DateTime.Now;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt => Id.Id?.CreationTime ?? DateTime.Now;

    public static Review Create(string content, int rating, IPhone16Id productId, IPhone16ModelId modelId, string OrderId, string orderItemId, string customerId, string customerUserName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);
        ArgumentOutOfRangeException.ThrowIfLessThan(rating, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(rating, 5);

        var review = new Review(ReviewId.Create())
        {
            ModelId = modelId,
            ProductId = productId,
            OrderId = OrderId,
            OrderItemId = orderItemId,
            CustomerId = customerId,
            CustomerUserName = customerUserName,
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
}
