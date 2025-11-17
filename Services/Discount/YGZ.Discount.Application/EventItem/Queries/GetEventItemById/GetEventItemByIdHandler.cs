using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
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
               .Select(ei => new EventItemResponse
               {
                   Id = ei.Id.Value.ToString() ?? string.Empty,
                   EventId = ei.EventId.Value.ToString() ?? string.Empty,
                   SkuId = ei.SkuId,
                   TenantId = ei.TenantId,
                   BranchId = ei.BranchId,
                   ModelName = ei.ModelName,
                   NormalizedModel = ei.NormalizedModel,
                   ColorName = ei.ColorName,
                   NormalizedColor = ei.NormalizedColor,
                   StorageName = ei.StorageName,
                   NormalizedStorage = ei.NormalizedStorage,
                   ProductClassification = ei.ProductClassification.Name,
                   ImageUrl = ei.ImageUrl,
                   DiscountType = ei.DiscountType.Name,
                   DiscountValue = ei.DiscountValue,
                   DiscountAmount = ei.DiscountAmount,
                   OriginalPrice = ei.OriginalPrice,
                   FinalPrice = ei.FinalPrice,
                   Stock = ei.Stock,
                   Sold = ei.Sold,
                   CreatedAt = ei.CreatedAt,
                   UpdatedAt = ei.UpdatedAt,
                   UpdatedBy = ei.UpdatedBy,
                   IsDeleted = ei.IsDeleted,
                   DeletedAt = ei.DeletedAt,
                   DeletedBy = ei.DeletedBy
               })
               .FirstOrDefaultAsync(cancellationToken);

            if (eventItem is null)
            {
                _logger.LogError("Event item with ID {EventItemId} not found", request.EventItemId);

                throw new Exception("Event item not found");
            }

            return eventItem;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving event item with ID {EventItemId}", request.EventItemId);
            throw;
        }
    }
}
