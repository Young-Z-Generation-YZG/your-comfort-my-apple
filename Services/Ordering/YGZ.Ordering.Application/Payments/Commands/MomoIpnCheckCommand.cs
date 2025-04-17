

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;

namespace YGZ.Ordering.Application.Payments.Commands;

public sealed record MomoIpnCheckCommand() : ICommand<OrderDetailsResponse>
{
    required public string PartnerCode { get; set; }
    required public string AccessKey { get; set; }
    required public string RequestId { get; set; }
    required public string Amount { get; set; }
    required public string OrderId { get; set; }
    required public string OrderInfo { get; set; }
    required public string OrderType { get; set; }
    required public string TransId { get; set; }
    required public string Message { get; set; }
    required public string LocalMessage { get; set; }
    required public string ResponseTime { get; set; }
    required public string ErrorCode { get; set; }
    required public string PayType { get; set; }
    required public string ExtraData { get; set; }
    required public string Signature { get; set; }
}
