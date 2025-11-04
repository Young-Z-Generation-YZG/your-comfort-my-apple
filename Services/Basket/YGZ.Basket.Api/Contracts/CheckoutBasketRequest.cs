using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Basket.Api.Contracts;

public sealed record CheckoutBasketRequest
{
    [Required]
    [JsonPropertyName("shipping_address")]
    public required ShippingAddressRequest ShippingAddress { get; init; }

    [Required]
    [JsonPropertyName("payment_method")]
    public required string PaymentMethod { get; init; }

    [JsonPropertyName("discount_code")]
    public string? DiscountCode { get; init; }
}


public sealed record ShippingAddressRequest
{
    [Required]
    [JsonPropertyName("contact_name")]
    public required string ContactName { get; init; }

    [Required]
    [JsonPropertyName("contact_phone_number")]
    public required string ContactPhoneNumber { get; init; }

    [Required]
    [JsonPropertyName("address_line")]
    public required string AddressLine { get; init; }

    [Required]
    [JsonPropertyName("district")]
    public required string District { get; init; }

    [Required]
    [JsonPropertyName("province")]
    public required string Province { get; init; }

    [Required]
    [JsonPropertyName("country")]
    public required string Country { get; init; }
}
