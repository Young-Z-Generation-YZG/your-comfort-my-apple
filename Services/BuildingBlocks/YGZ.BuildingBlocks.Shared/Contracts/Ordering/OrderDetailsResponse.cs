
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
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

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record OrderItemRepsonse()
{
    required public string OrderItemId { get; set; }
    required public string ProductId { get; set; }
    required public string ProductName { get; set; }
    required public string ProductImage { get; set; }
    required public string ProductColorName { get; set; }
    required public decimal ProductUnitPrice { get; set; }
    required public int quantity { get; set; }
    public PromotionResponse? Promotion { get; set; } = null;
    required public decimal SubTotalAmount { get; set; }
}