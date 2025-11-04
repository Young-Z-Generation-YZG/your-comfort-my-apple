using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Basket.Api.Contracts;

public sealed record CheckoutBasketWithBlockchainRequest
{
    [Required]
    [JsonPropertyName("crypto_uuid")]
    public required string CryptoUUID { get; init; }

    [Required]
    [JsonPropertyName("shipping_address")]
    public required ShippingAddressRequest ShippingAddress { get; init; }

    [Required]
    [JsonPropertyName("payment_method")]
    public required string PaymentMethod { get; init; }

    [JsonPropertyName("discount_code")]
    public string? DiscountCode { get; init; }

    [JsonPropertyName("customer_public_key")]
    public string? CustomerPublicKey { get; init; }

    [JsonPropertyName("tx")]
    public string? Tx { get; init; }
}
