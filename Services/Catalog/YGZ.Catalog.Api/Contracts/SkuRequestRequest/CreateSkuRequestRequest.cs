using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.SkuRequestRequest;

public class CreateSkuRequestRequest
{
    [Required]
    [JsonPropertyName("sender_user_id")]
    public required string SenderUserId { get; init; }

    [Required]
    [JsonPropertyName("from_branch_id")]
    public required string FromBranchId { get; init; }

    [Required]
    [JsonPropertyName("to_branch_id")]
    public required string ToBranchId { get; init; }

    [Required]
    [JsonPropertyName("sku_id")]
    public required string SkuId { get; init; }

    [Required]
    [JsonPropertyName("request_quantity")]
    public required int RequestQuantity { get; init; }
}
