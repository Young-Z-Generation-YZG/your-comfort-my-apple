using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Ordering;

[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record class OrderResponse
{
    public string OrderId { get; init; }
    public string OrderCode { get; init; }
    public string OrderCustomerEmail { get; init; }
    public string OrderStatus { get; init; }
    public string OrderPaymentMethod { get; init; }
    public ShippingAddressResponse OrderShippingAddress { get; init; }
    required public int OrderItemsCount { get; set; }
    public decimal OrderSubTotalAmount { get; init; }
    public decimal OrderDiscountAmount { get; init; }
    public decimal OrderTotalAmount { get; init; }
    public DateTime OrderCreatedAt { get; init; }
    public DateTime OrderUpdatedAt { get; init; }
    public string? OrderLastModifiedBy { get; init; }
}

[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record ShippingAddressResponse
{
    public string ContactName { get; init; }
    public string ContactEmail { get; init; }
    public string ContactPhoneNumber { get; init; }
    public string ContactAddressLine { get; init; }
    public string ContactDistrict { get; init; }
    public string ContactProvince { get; init; }
    public string ContactCountry { get; init; }
}