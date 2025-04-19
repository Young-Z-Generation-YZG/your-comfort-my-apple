
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Domain.Products.Iphone16;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Application.Abstractions.Data;

public interface IIPhone16ModelRepository : IMongoRepository<IPhone16Model, IPhone16ModelId>
{

}
