using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Tenants.Entities;

namespace YGZ.Catalog.Domain.Tenants.Events;

public sealed record ReservedQuantityDeductedDomainEvent(SKU Sku) : IDomainEvent { }
