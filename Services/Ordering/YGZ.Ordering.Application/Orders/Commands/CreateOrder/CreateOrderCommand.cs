
using YGZ.Ordering.Application.Core.Abstractions.Messaging;

namespace YGZ.Ordering.Application.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(string Customer_id,
                                        AddressCommand Shipping_address,
                                        AddressCommand Billing_address,
                                        string Payment_status,
                                        string Payment_type,
                                        List<OrderLineCommand> Order_lines) : ICommand<bool> { }


public sealed record AddressCommand(string Contact_name,
                                    string Contact_email,
                                    string Contact_phone_number,
                                    string Address_line,
                                    string District,
                                    string Province,
                                    string Country) { }

public sealed record OrderLineCommand(string Product_id,
                                      string Name,
                                      string Model,
                                      string Color,
                                      int Storage,
                                      string Slug,
                                      decimal Price,
                                      int Quantity) { }