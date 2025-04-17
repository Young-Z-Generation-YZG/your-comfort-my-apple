using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Ordering.Api.Contracts;

public class MomoIpnCheckRequest
{
    [Required]
    [JsonPropertyName("momo_partnerCode")]
    required public string PartnerCode { get; set; }

    [Required]
    [JsonPropertyName("momo_accessKey")]
    required public string AccessKey { get; set; }

    [Required]
    [JsonPropertyName("momo_requestId")]
    required public string RequestId { get; set; }

    [Required]
    [JsonPropertyName("momo_amount")]
    required public string Amount { get; set; }

    [Required]
    [JsonPropertyName("momo_orderId")]
    required public string OrderId { get; set; }

    [Required]
    [JsonPropertyName("momo_orderInfo")]
    required public string OrderInfo { get; set; }

    [Required]
    [JsonPropertyName("momo_orderType")]
    required public string OrderType { get; set; }

    [Required]
    [JsonPropertyName("momo_transId")]
    required public string TransId { get; set; }

    [Required]
    [JsonPropertyName("momo_message")]
    required public string Message { get; set; }

    [Required]
    [JsonPropertyName("momo_localMessage")]
    required public string LocalMessage { get; set; }

    [Required]
    [JsonPropertyName("momo_responseTime")]
    required public string ResponseTime { get; set; }

    [Required]
    [JsonPropertyName("momo_errorCode")]
    required public string ErrorCode { get; set; }

    [Required]
    [JsonPropertyName("momo_payType")]
    required public string PayType { get; set; }

    [JsonPropertyName("momo_extraData")]
    public string? ExtraData { get; set; }

    [Required]
    [JsonPropertyName("momo_signature")]
    required public string Signature { get; set; }
}
