
using YGZ.Ordering.Application.Common.Commands;
using YGZ.Ordering.Application.Core.Abstractions.Messaging;
using static YGZ.Ordering.Domain.Core.Enums.Enums;

namespace YGZ.Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(string User_id,
                                    AddressCommand Shipping_address,
                                    AddressCommand Billing_address,
                                    string Payment_status,
                                    PaymentType Payment_type,
                                    List<OrderLineCommand> Order_lines) : ICommand<bool> { }

