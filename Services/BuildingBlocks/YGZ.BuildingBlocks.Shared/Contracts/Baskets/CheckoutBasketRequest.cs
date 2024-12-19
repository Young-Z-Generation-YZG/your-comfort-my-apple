

namespace YGZ.BuildingBlocks.Shared.Contracts.Baskets;

public record CheckoutBasketRequest(string UserId,
                                    string Contact_name,
                                    string Contact_phone_number,
                                    string Contact_email,
                                    string Address_line,
                                    string District,
                                    string Province,
                                    string Country,
                                    string Payment_type) { }

