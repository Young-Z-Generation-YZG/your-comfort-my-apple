using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Profiles;


public sealed record UpdateProfileRequest
{
    [JsonPropertyName("first_name")] 
    public string? FirstName { get; init; }
    
    [JsonPropertyName("last_name")] 
    public string? LastName { get; init; }
    
    [JsonPropertyName("phone_number")] 
    public string? PhoneNumber { get; init; }
    
    [JsonPropertyName("birthday")] 
    public string? BirthDay { get; init; }
    
    [JsonPropertyName("gender")] 
    public string? Gender { get; init; }
    
}
