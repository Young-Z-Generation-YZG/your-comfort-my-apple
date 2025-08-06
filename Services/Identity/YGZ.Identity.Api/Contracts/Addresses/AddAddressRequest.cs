using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Addresses;

public sealed record AddAddressRequest
{
    [JsonPropertyName("label")] 
    public required string Label { get; init; }
    
    [JsonPropertyName("contact_name")] 
    public required string ContactName { get; init; }
    
    [JsonPropertyName("contact_phone_number")] 
    public required string ContactPhoneNumber { get; init; }
    
    [JsonPropertyName("address_line")] 
    public required string AddressLine { get; init; }
    
    [JsonPropertyName("district")] 
    public required string District { get; init; }
    
    [JsonPropertyName("province")] 
    public required string Province { get; init; }
    
    [JsonPropertyName("country")] 
    public required string Country { get; init; }
    
}
