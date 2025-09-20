

using YGZ.Catalog.Domain.Products.Iphone.Entities;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;

namespace YGZ.Catalog.Domain.Core.Abstractions.Data;

public interface IReviewRepository : IMongoRepository<Review, ReviewId> { }