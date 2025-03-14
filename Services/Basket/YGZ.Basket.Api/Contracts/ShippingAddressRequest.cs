
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Basket.Api.Contracts;

#pragma warning disable CS8618
public sealed record ShippingAddressRequest
{
    [Required]
    [JsonPropertyName("contact_name")]
    public string ContactName { get; set; }

    [Required]
    [JsonPropertyName("contact_phone_number")]
    public string ContactPhoneNumber { get; set; }

    [Required]
    [JsonPropertyName("address_line")]
    public string AddressLine { get; set; }

    [Required]
    [JsonPropertyName("district")]
    public string District { get; set; }

    [Required]
    [JsonPropertyName("province")]
    public string Province { get; set; }

    [Required]
    [JsonPropertyName("country")]
    public string Country { get; set; }
}
