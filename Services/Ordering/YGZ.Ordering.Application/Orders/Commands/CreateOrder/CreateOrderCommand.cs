
using YGZ.Ordering.Application.Common.Commands;
using YGZ.Ordering.Application.Core.Abstractions.Messaging;
using static YGZ.Ordering.Domain.Core.Enums.Enums;

namespace YGZ.Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(string UserId,
                                    AddressCommand ShippingAddress,
                                    AddressCommand BillingAddress,
                                    string PaymentStatus,
                                    string PaymentType,
                                    List<OrderLineCommand> OrderLines) : ICommand<bool> { }

