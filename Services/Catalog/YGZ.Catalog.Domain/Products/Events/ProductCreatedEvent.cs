
using YGZ.Catalog.Domain.Core.Abstractions;

namespace YGZ.Catalog.Domain.Products.Events;

public sealed record ProductCreatedEvent(Product Product) : IDomainEvent { }
