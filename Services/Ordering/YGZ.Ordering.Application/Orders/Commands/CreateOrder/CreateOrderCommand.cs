
using YGZ.Ordering.Application.Common.Commands;
using YGZ.Ordering.Application.Core.Abstractions.Messaging;

namespace YGZ.Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(string User_id,
                                    AddressCommand Shipping_address,
                                    AddressCommand Billing_address,
                                    string Payment_status,
                                    string Payment_type,
                                    List<OrderLineCommand> Order_lines) : ICommand<bool> { }

