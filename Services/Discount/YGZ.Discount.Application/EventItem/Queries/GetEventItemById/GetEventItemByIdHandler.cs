using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Domain.Core.Errors;
using YGZ.Discount.Domain.Event.ValueObjects;
using YGZ.Discount.Infrastructure.Persistence;

namespace YGZ.Discount.Application.EventItem.Queries.GetEventItemById;

public class GetEventItemByIdHandler : IQueryHandler<GetEventItemByIdQuery, EventItemResponse>
{
    private readonly DiscountDbContext _dbContext;
    private readonly ILogger<GetEventItemByIdHandler> _logger;

    public GetEventItemByIdHandler(DiscountDbContext dbContext,
                                   ILogger<GetEventItemByIdHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<EventItemResponse>> Handle(GetEventItemByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var eventItem = await _dbContext.EventItems
               .Where(ei => ei.Id == EventItemId.Of(Guid.Parse(request.EventItemId)))
               .Select(ei => ei.ToResponse())
               .FirstOrDefaultAsync(cancellationToken);

            if(eventItem is null) {
                _logger.LogError(":::[QueryHandler:{QueryHandler}][Method:{MethodName}:::] Error message: {ErrorMessage}, Parameters: {@Parameters}", 
                    nameof(GetEventItemByIdHandler), nameof(DiscountDbContext), Errors.EventItem.NotFound.Message, request);

                return Errors.EventItem.NotFound;
            }

            return eventItem;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ":::[QueryHandler:{QueryHandler}][Method:{MethodName}]::: Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(GetEventItemByIdHandler), nameof(DiscountDbContext), ex.Message, request);

            throw;
        }
    }
}
