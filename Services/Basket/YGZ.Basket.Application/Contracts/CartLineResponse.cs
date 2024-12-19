
namespace YGZ.Basket.Application.Contracts;

public sealed record CartLineResponse(string ProductId,
                                      string Model,
                                      string Color,
                                      int Storage,
                                      string Primary_image_url,
                                      int Quantity,
                                      decimal Price,
                                      decimal Discount_amount,
                                      decimal Sub_total) { }

