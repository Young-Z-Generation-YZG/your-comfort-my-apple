using YGZ.Catalog.Domain.Core.Abstractions;

namespace YGZ.Catalog.Domain.Requests.SkuRequest.Events;

public sealed record SkuRequestApprovedDomainEvent(SkuRequest SkuRequest) : IDomainEvent;
