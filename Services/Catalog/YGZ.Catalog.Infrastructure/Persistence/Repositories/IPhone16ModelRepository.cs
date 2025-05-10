
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Iphone16;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;
using YGZ.Catalog.Infrastructure.Persistence.Interceptors;
using YGZ.Catalog.Infrastructure.Settings;

namespace YGZ.Catalog.Infrastructure.Persistence.Repositories;

public class IPhone16ModelRepository : MongoRepository<IPhone16Model, IPhone16ModelId>, IIPhone16ModelRepository
{
    private readonly IDispatchDomainEventInterceptor _dispatchDomainEventInterceptor;
    private readonly ILogger<IPhone16ModelRepository> _logger;

    public IPhone16ModelRepository(IOptions<MongoDbSettings> options, IDispatchDomainEventInterceptor dispatchDomainEventInterceptor, ILogger<IPhone16ModelRepository> logger) : base(options, logger, dispatchDomainEventInterceptor)
    {
        _dispatchDomainEventInterceptor = dispatchDomainEventInterceptor;
        _logger = logger;
    }

    public override async Task<Result<bool>> InsertOneAsync(IPhone16Model document)
    {
        return await base.InsertOneAsync(document);
    }
}
