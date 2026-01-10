using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.SkuRequestRequest;

public class UpdateSkuRequestRequest
{
    [Required]
    [JsonPropertyName("state")]
    public required string State { get; init; }
}
