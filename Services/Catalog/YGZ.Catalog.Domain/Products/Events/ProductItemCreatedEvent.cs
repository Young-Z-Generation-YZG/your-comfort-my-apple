
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Products.Entities;

namespace YGZ.Catalog.Domain.Products.Events;

public sealed record ProductItemCreatedEvent(ProductItem productItem) : IDomainEvent { }

