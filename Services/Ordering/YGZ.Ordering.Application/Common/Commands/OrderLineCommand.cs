

namespace YGZ.Ordering.Application.Common.Commands;

public record OrderLineCommand(string Product_id,
                               string Name,
                               string Model,
                               string Color,
                               int Storage,
                               string Slug,
                               decimal Price,
                               int Quantity) { }