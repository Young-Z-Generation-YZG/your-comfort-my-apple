


using YGZ.Basket.Application.Core.Abstractions.Messaging;

namespace YGZ.Basket.Application.Baskets.Commands.CheckoutBasket;

public record CheckoutBasketCommand(string UserId,
                                    string ContactName,
                                    string ContactPhoneNumber,
                                    string ContactEmail,
                                    string AddressLine,
                                    string District,
                                    string Province,
                                    string Country,
                                    string PaymentType) : ICommand<string> { }



