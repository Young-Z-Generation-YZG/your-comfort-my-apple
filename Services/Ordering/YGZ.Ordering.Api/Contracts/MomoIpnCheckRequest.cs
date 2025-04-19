using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Ordering.Api.Contracts;

public class MomoIpnCheckRequest
{
    [Required]
    [JsonPropertyName("momo_PartnerCode")]
    required public string PartnerCode { get; set; }

    [Required]
    [JsonPropertyName("momo_AccessKey")]
    required public string AccessKey { get; set; }

    [Required]
    [JsonPropertyName("momo_RequestId")]
    required public string RequestId { get; set; }

    [Required]
    [JsonPropertyName("momo_Amount")]
    required public string Amount { get; set; }

    [Required]
    [JsonPropertyName("momo_OrderId")]
    required public string OrderId { get; set; }

    [Required]
    [JsonPropertyName("momo_OrderInfo")]
    required public string OrderInfo { get; set; }

    [Required]
    [JsonPropertyName("momo_OrderType")]
    required public string OrderType { get; set; }

    [Required]
    [JsonPropertyName("momo_TransId")]
    required public string TransId { get; set; }

    [Required]
    [JsonPropertyName("momo_Message")]
    required public string Message { get; set; }

    [Required]
    [JsonPropertyName("momo_LocalMessage")]
    required public string LocalMessage { get; set; }

    [Required]
    [JsonPropertyName("momo_ResponseTime")]
    required public string ResponseTime { get; set; }

    [Required]
    [JsonPropertyName("momo_ErrorCode")]
    required public string ErrorCode { get; set; }

    [Required]
    [JsonPropertyName("momo_PayType")]
    required public string PayType { get; set; }

    [JsonPropertyName("momo_ExtraData")]
    public string? ExtraData { get; set; }

    [Required]
    [JsonPropertyName("momo_Signature")]
    required public string Signature { get; set; }
}
