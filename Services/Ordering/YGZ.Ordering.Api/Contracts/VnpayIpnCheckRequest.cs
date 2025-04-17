using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Ordering.Api.Contracts;

public class VnpayIpnCheckRequest
{
    [Required]
    [JsonPropertyName("vnp_Amount")]
    required public string Amount { get; set; }

    [Required]
    [JsonPropertyName("vnp_OrderInfo")]
    required public string OrderInfo { get; set; }

    [Required]
    [JsonPropertyName("vnp_BankCode")]
    required public string BankCode { get; set; }

    [Required]
    [JsonPropertyName("vnp_BankTranNo")]
    required public string BankTranNo { get; set; }

    [Required]
    [JsonPropertyName("vnp_CardType")]
    required public string CardType { get; set; }

    [Required]
    [JsonPropertyName("vnp_PayDate")]
    required public string PayDate { get; set; }

    [Required]
    [JsonPropertyName("vnp_ResponseCode")]
    required public string ResponseCode { get; set; }

    [Required]
    [JsonPropertyName("vnp_TmnCode")]
    required public string TmnCode { get; set; }

    [Required]
    [JsonPropertyName("vnp_TransactionNo")]
    required public string TransactionNo { get; set; }

    [Required]
    [JsonPropertyName("vnp_TransactionStatus")]
    required public string TransactionStatus { get; set; }

    [Required]
    [JsonPropertyName("vnp_TxnRef")]
    required public string TxnRef { get; set; }

    [Required]
    [JsonPropertyName("vnp_SecureHash")]
    required public string SecureHash { get; set; }
}