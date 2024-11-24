
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Products;

namespace YGZ.Catalog.Persistence.Data;

public class ProductRepository : BaseRepository<Product>
{
    public ProductRepository(IMongoContext context) : base(context) {}

}
