using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Products.Iphone.Entities;

namespace YGZ.Catalog.Domain.Products.Iphone.Events;

public sealed record ReviewUpdatedDomainEvent(Review OldReview, Review NewReview) : IDomainEvent { }