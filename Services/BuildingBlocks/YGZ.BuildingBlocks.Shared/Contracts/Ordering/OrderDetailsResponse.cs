
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Ordering;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record OrderDetailsResponse()
{
    required public string OrderId { get; set; }
    required public string OrderCode { get; set; }
    required public string OrderCustomerEmail { get; set; }
    required public string OrderStatus { get; set; }
    required public string OrderPaymentMethod { get; set; }
    required public ShippingAddressResponse OrderShippingAddress { get; set; }
    required public List<OrderItemRepsonse> OrderItems { get; set; } = new List<OrderItemRepsonse>();
    required public decimal OrderSubTotalAmount { get; set; }
    required public decimal OrderDiscountAmount { get; set; }
    required public decimal OrderTotalAmount { get; set; }
    required public DateTime OrderCreatedAt { get; set; }
    required public DateTime OrderUpdatedAt { get; set; }
    required public string? OrderLastModifiedBy { get; set; }
}