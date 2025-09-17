


using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone.Entities;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;
using YGZ.Catalog.Infrastructure.Persistence.Interceptors;
using YGZ.Catalog.Infrastructure.Settings;

namespace YGZ.Catalog.Infrastructure.Persistence.Repositories;

public class ReviewRepository : MongoRepository<Review, ReviewId>, IReviewRepository
{
    private readonly IDispatchDomainEventInterceptor _dispatchDomainEventInterceptor;
    private readonly ILogger<ReviewRepository> _logger;

    public ReviewRepository(IOptions<MongoDbSettings> options, ILogger<ReviewRepository> logger, IDispatchDomainEventInterceptor dispatchDomainEventInterceptor) : base(options, logger, dispatchDomainEventInterceptor)
    {
        _dispatchDomainEventInterceptor = dispatchDomainEventInterceptor;
        _logger = logger;
    }

    //public override async Task<Result<bool>> InsertOneAsync(Review document)
    //{
    //    var domainEventEntities = document.DomainEvents.ToList();
    //    document.ClearDomainEvents();

    //    foreach (var domainEvent in domainEventEntities)
    //    {
    //        await _dispatchDomainEventInterceptor.BeforeInsert(domainEvent);
    //    }
    //    try
    //    {
    //        return await base.InsertOneAsync(document);
    //    }
    //    catch
    //    {
    //        return Errors.Review.AddReviewFailure;
    //    }
    //}
}
