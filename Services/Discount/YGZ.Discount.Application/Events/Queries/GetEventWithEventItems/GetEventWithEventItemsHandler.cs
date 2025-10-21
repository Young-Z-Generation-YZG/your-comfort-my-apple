using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Domain.Core.Errors;
using YGZ.Discount.Infrastructure.Persistence;

namespace YGZ.Discount.Application.Events.Queries.GetEventWithEventItems;

public class GetEventWithEventItemsHandler : IQueryHandler<GetEventWithEventItemsQuery, EventWithEventItemsResponse>
{
    private readonly DiscountDbContext _dbContext;
    private readonly ILogger<GetEventWithEventItemsHandler> _logger;

    public GetEventWithEventItemsHandler(
        DiscountDbContext dbContext,
        ILogger<GetEventWithEventItemsHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<EventWithEventItemsResponse>> Handle(
        GetEventWithEventItemsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Get the first active event with its event items
            var eventEntity = await _dbContext.Events
                .Include(e => e.EventItems)
                .Where(e => !e.IsDeleted)
                .OrderByDescending(e => e.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (eventEntity is null)
            {
                _logger.LogWarning("No active event found");

                return Errors.Event.EventNotFound;
            }

            // Map to response
            var eventResponse = new EventResponse
            {
                Id = eventEntity.Id.Value.ToString()!,
                Title = eventEntity.Title,
                Description = eventEntity.Description,
                StartDate = eventEntity.StartDate,
                EndDate = eventEntity.EndDate
            };

            var eventItemResponses = eventEntity.EventItems
                .Where(ei => !ei.IsDeleted)
                .Select(ei => new EventItemResponse
                {
                    Id = ei.Id.Value.ToString()!,
                    EventId = ei.EventId.Value.ToString()!,
                    ModelName = ei.ModelName,
                    NormalizedModel = ei.NormalizedModel,
                    ColorName = ei.ColorName,
                    NormalizedColor = ei.NormalizedColor,
                    ColorHexCode = ei.ColorHaxCode,
                    StorageName = ei.StorageName,
                    NormalizedStorage = ei.NormalizedStorage,
                    ProductClassification = ei.ProductClassification.Name,
                    ImageUrl = ei.ImageUrl,
                    DiscountType = ei.DiscountType.Name,
                    DiscountValue = ei.DiscountValue,
                    OriginalPrice = ei.OriginalPrice,
                    Stock = ei.Stock,
                    Sold = ei.Sold
                })
                .ToList();

            var response = new EventWithEventItemsResponse
            {
                Event = eventResponse,
                EventItems = eventItemResponses
            };

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching event with event items");

            throw;
        }
    }
}
