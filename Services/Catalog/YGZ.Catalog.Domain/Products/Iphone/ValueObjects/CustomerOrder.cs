using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Iphone.ValueObjects;

public class CustomerOrder : ValueObject
{
    [BsonElement("user_id")]
    public string UserId { get; init; }

    [BsonElement("order_id")]
    public string OrderId { get; init; }

    [BsonElement("order_item_id")]
    public string OrderItemId { get; init; }

    [BsonElement("full_name")]
    public string FullName { get; init; }

    private CustomerOrder(string userId, string orderId, string orderItemId, string fullName)
    {
        UserId = userId;
        OrderId = orderId;
        OrderItemId = orderItemId;
        FullName = fullName;
    }

    public static CustomerOrder Create(string userId, string orderId, string orderItemId, string fullName)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(orderId))
        {
            throw new ArgumentException("Order ID cannot be null or empty", nameof(orderId));
        }

        if (string.IsNullOrWhiteSpace(orderItemId))
        {
            throw new ArgumentException("Order Item ID cannot be null or empty", nameof(orderItemId));
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Full name cannot be null or empty", nameof(fullName));
        }

        return new CustomerOrder(userId, orderId, orderItemId, fullName);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return UserId;
        yield return OrderId;
        yield return OrderItemId;
        yield return FullName;
    }
}
