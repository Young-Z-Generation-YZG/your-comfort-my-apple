

namespace YGZ.Ordering.Application.Common.Commands;

public record OrderLineCommand(string ProductTd,
                               string Model,
                               string Color,
                               int Storage,
                               string Slug,
                               double Price,
                               int Quantity,
                               double SubTotal) { }