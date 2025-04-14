using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Basket.Api.Contracts;

#pragma warning disable CS8618

public class CheckoutBasketRequest
{
    [Required]
    [JsonPropertyName("shipping_address")]
    public ShippingAddressRequest ShippingAddress { get; set; }

    [Required]
    [JsonPropertyName("payment_method")]
    public string PaymentMethod { get; set; }

    [JsonPropertyName("discount_code")]
    public string? DiscountCode { get; set; } = null;

    [Required]
    [JsonPropertyName("discount_amount")]
    public decimal DiscountAmount { get; set; } = 0;

    [Required]
    [JsonPropertyName("sub_total_amount")]
    public decimal SubTotalAmount { get; set; }

    [Required]
    [JsonPropertyName("total_amount")]
    public decimal TotalAmount { get; set; }
}
