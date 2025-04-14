using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Basket.Api.Contracts;

public class IpnCheckRequest
{
    [Required]
    public string vnp_Amount { get; set; }

    [Required]
    public string vnp_OrderInfo { get; set; }

    [Required]
    public string vnp_BankCode { get; set; }

    [Required]
    public string vnp_BankTranNo { get; set; }

    [Required]
    public string vnp_CardType { get; set; }

    [Required]
    public string vnp_PayDate { get; set; }

    [Required]
    public string vnp_ResponseCode { get; set; }

    [Required]
    public string vnp_TmnCode { get; set; }

    [Required]
    public string vnp_TransactionNo { get; set; }

    [Required]
    public string vnp_TransactionStatus { get; set; }

    [Required]
    public string vnp_TxnRef { get; set; }

    [Required]
    public string vnp_SecureHash { get; set; }
}