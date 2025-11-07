using YGZ.Catalog.Domain.Core.Abstractions;

namespace YGZ.Catalog.Domain.Products.ProductModels.Events;

public sealed record ProductModelCreatedDomainEvent(ProductModel ProductModel) : IDomainEvent { }