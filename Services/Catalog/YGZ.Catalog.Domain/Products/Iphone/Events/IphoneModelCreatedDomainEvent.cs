
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Products.ProductModels;

namespace YGZ.Catalog.Domain.Products.Iphone.Events;

public sealed record IphoneModelCreatedDomainEvent(IphoneModel IphoneModel, ProductModel ProductModel) : IDomainEvent { }
