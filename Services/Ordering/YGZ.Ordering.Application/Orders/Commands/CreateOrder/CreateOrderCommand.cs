using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.Ordering.Application.Orders.Commands.Common;
using YGZ.Ordering.Domain.Core.Enums;

namespace YGZ.Ordering.Application.Orders.Commands.CreateOrder;

#pragma warning disable CS8618

public sealed record CreateOrderCommand(string CustomerId,
                                        string CustomerEmail,
                                        ShippingAddressCommand ShippingAddress,
                                        List<OrderItemCommand> Orders,
                                        string PaymentMethod,
                                        decimal DiscountAmount,
                                        decimal SubTotalAmount,
                                        decimal TotalAmount) : ICommand<bool>
{ }