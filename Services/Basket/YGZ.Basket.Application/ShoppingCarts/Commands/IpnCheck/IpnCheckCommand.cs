﻿
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.IpnCheck;

public sealed record IpnCheckCommand() : ICommand<string>
{
    required public string Amount { get; set; }
    required public string OrderInfo { get; set; }
    required public string BankCode { get; set; }
    required public string BankTranNo { get; set; }
    required public string CardType { get; set; }
    required public string PayDate { get; set; }
    required public string ResponseCode { get; set; }
    required public string TmnCode { get; set; }
    required public string TransactionNo { get; set; }
    required public string TransactionStatus { get; set; }
    required public string TxnRef { get; set; }
    required public string SecureHash { get; set; }
}
