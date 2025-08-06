using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Ordering.Api.Contracts.Common;

namespace YGZ.Ordering.Api.Contracts;

#pragma warning disable CS8618

[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record CreateOrderRequest
{
    [Required]
    //[JsonPropertyName("orders")]
    public List<OrderItemRequest> Orders { get; set; }

    [Required]
    //[JsonPropertyName("shipping_address")]
    public ShippingAddressRequest ShippingAddress { get; set; }

    [Required]
    //[JsonPropertyName("payment_method")]
    public string PaymentMethod { get; set; }

    [Required]
    //[JsonPropertyName("discount_amount")]
    public decimal DiscountAmount { get; set; }

    [Required]
    //[JsonPropertyName("sub_total")]
    public decimal SubTotalAmount { get; set; }

    [Required]
    //[JsonPropertyName("total")]
    public decimal TotalAmount { get; set; }
}
