using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.TenantRequest;

public class CreateTenantRequest
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("sub_domain")]
    public required string SubDomain { get; init; }

    [JsonPropertyName("branch_address")]
    public required string BranchAddress { get; init; }

    [JsonPropertyName("tenant_type")]
    public required string TenantType { get; init; }

    [JsonPropertyName("tenant_description")]
    public string? TenantDescription { get; set; }

    [JsonPropertyName("branch_description")]
    public string? BranchDescription { get; set; }
}
