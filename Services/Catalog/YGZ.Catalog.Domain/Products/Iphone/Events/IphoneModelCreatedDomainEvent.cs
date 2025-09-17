
using YGZ.Catalog.Domain.Core.Abstractions;

namespace YGZ.Catalog.Domain.Products.Iphone.Events;

public sealed record IphoneModelCreatedDomainEvent(IphoneModel IphoneModel) : IDomainEvent { }
