

namespace YGZ.Ordering.Application.Common.Commands;

public record OrderLineCommand(string ProductTd,
                               string Model,
                               string Color,
                               int Storage,
                               string ProductImageUrl,
                               decimal Price,
                               int Quantity,
                               decimal DiscountAmount,
                               decimal SubTotal) { }