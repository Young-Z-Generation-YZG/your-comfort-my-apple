

using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;

namespace YGZ.Catalog.Domain.Products.Iphone16.Events;

public sealed record ReviewCreatedDomainEvent(Review Review) : IDomainEvent { }
