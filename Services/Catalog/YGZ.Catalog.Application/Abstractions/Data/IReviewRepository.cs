

using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Application.Abstractions.Data;

public interface IReviewRepository : IMongoRepository<Review, ReviewId>
{

}
