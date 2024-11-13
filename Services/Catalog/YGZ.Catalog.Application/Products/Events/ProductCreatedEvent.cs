

using MassTransit;

namespace YGZ.Catalog.Application.Products.Events;

public record ProductCreatedEvent
{
    public string ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public string Description { get; init; }
}
