
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;

namespace YGZ.Ordering.Application.Payments.Commands;

public sealed record VnpayIpnCheckCommand() : ICommand<OrderDetailsResponse>
{
    public required string Amount { get; init; }
    public required string OrderInfo { get; init; }
    public required string BankCode { get; init; }
    public required string BankTranNo { get; init; }
    public required string CardType { get; init; }
    public required string PayDate { get; init; }
    public required string ResponseCode { get; init; }
    public required string TmnCode { get; init; }
    public required string TransactionNo { get; init; }
    public required string TransactionStatus { get; init; }
    public required string TxnRef { get; init; }
    public required string SecureHash { get; init; }
}
