using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Products.Iphone.Entities;

namespace YGZ.Catalog.Domain.Products.Iphone.Events;

public sealed record ReviewCreatedDomainEvent(Review Review) : IDomainEvent { }
