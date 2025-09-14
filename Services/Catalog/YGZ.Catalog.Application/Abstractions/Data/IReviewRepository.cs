

using YGZ.Catalog.Domain.Products.Iphone.Entities;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;

namespace YGZ.Catalog.Application.Abstractions.Data;

public interface IReviewRepository : IMongoRepository<Review, ReviewId>
{

}
