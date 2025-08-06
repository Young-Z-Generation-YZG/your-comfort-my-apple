using System.Text.Json.Serialization;

namespace YGZ.Identity.Api.Contracts.Profiles;


public sealed record UpdateProfileRequest
{
    [JsonPropertyName("first_name")] 
    public required string FirstName { get; init; }
    
    [JsonPropertyName("last_name")] 
    public required string LastName { get; init; }
    
    [JsonPropertyName("phone_number")] 
    public required string PhoneNumber { get; init; }
    
    [JsonPropertyName("birthday")] 
    public required string BirthDay { get; init; }
    
    [JsonPropertyName("gender")] 
    public required string Gender { get; init; }
    
}
