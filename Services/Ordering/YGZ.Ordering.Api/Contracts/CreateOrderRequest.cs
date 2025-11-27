using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Ordering.Api.Contracts.Common;

namespace YGZ.Ordering.Api.Contracts;


[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record CreateOrderRequest
{
    [Required]
    public List<OrderItemRequest> Orders { get; set; }

    [Required]
    public ShippingAddressRequest ShippingAddress { get; set; }

    [Required]
    public string PaymentMethod { get; set; }

    [Required]
    public decimal DiscountAmount { get; set; }

    [Required]
    public decimal SubTotalAmount { get; set; }

    [Required]
    public decimal TotalAmount { get; set; }
}
