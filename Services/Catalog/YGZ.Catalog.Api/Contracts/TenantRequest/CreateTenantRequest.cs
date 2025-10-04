using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.TenantRequest;

public class CreateTenantRequest
{
    [Required]
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [Required]
    [JsonPropertyName("branch_address")]
    public required string BranchAddress { get; init; }

    [JsonPropertyName("tenant_description")]
    public string? TenantDescription { get; set; }

    [JsonPropertyName("branch_description")]
    public string? BranchDescription { get; set; }
}
