using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.Ordering.Application.Orders.Commands.Common;
using YGZ.Ordering.Domain.Core.Enums;

namespace YGZ.Ordering.Application.Orders.Commands.CreateOrder;

#pragma warning disable CS8618

public sealed record CreateOrderCommand(List<OrderItemCommand> Orders,
                                        ShippingAddressCommand ShippingAddress,
                                        string PaymentMethod,
                                        decimal DiscountAmount,
                                        decimal SubTotal,
                                        decimal Total) : ICommand<bool> { }