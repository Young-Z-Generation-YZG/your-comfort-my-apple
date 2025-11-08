using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.Reviews;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Iphone.ValueObjects;

public class OrderInfo : ValueObject
{
    [BsonElement("order_id")]
    public string OrderId { get; init; }

    [BsonElement("order_item_id")]
    public string OrderItemId { get; init; }

    public OrderInfo(string orderId, string orderItemId)
    {
        OrderId = orderId;
        OrderItemId = orderItemId;
    }

    public static OrderInfo Create(string orderId, string orderItemId)
    {
        return new OrderInfo(orderId, orderItemId);
    }


    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return OrderId;
        yield return OrderItemId;
    }

    public OrderInfoResponse ToResponse()
    {
        return new OrderInfoResponse
        {
            OrderId = OrderId,
            OrderItemId = OrderItemId
        };
    }
}
