// using Microsoft.EntityFrameworkCore;
// using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
// using YGZ.BuildingBlocks.Shared.Abstractions.Result;
// using YGZ.Discount.Application.Events.Queries.GetEventWithProductSKUs;
// using YGZ.Discount.Domain.Abstractions.Data;
// using YGZ.Discount.Domain.Event;
// using YGZ.Discount.Domain.Event.ValueObjects;

// namespace YGZ.Discount.Application.Events.Queries.GetEventWithProductSKUs;

// public class GetEventWithProductSKUsQueryHandler : IQueryHandler<GetEventWithProductSKUsQuery, EventWithProductSKUsResponse>
// {
//     private readonly IGenericRepository<Event, EventId> _repository;

//     public GetEventWithProductSKUsQueryHandler(IGenericRepository<Event, EventId> repository)
//     {
//         _repository = repository;
//     }

//     public async Task<Result<EventWithProductSKUsResponse>> Handle(GetEventWithProductSKUsQuery request, CancellationToken cancellationToken)
//     {
//         var @event = await _repository.GetQueryable()
//             .Include(e => e.EventProductSKUs.Where(ep => !ep.IsDeleted))
//             .FirstOrDefaultAsync(e => e.Id == request.EventId && !e.IsDeleted, cancellationToken);

//         if (@event is null)
//         {
//             return Result.Failure<EventWithProductSKUsResponse>(Error.NotFound("Event", request.EventId.Value));
//         }

//         var response = new EventWithProductSKUsResponse
//         {
//             EventId = @event.Id,
//             Title = @event.Title,
//             Description = @event.Description,
//             State = @event.State,
//             StartDate = @event.StartDate,
//             EndDate = @event.EndDate,
//             CreatedAt = @event.CreatedAt,
//             UpdatedAt = @event.UpdatedAt,
//             EventProductSKUs = @event.EventProductSKUs.Select(ep => new EventProductSKUResponse
//             {
//                 Id = ep.Id,
//                 SKUId = ep.SKUId,
//                 ProductType = ep.ProductType,
//                 AvailableQuantity = ep.AvailableQuantity,
//                 DiscountType = ep.DiscountType,
//                 DiscountValue = ep.DiscountValue,
//                 ImageUrl = ep.ImageUrl,
//                 Slug = ep.Slug,
//                 CreatedAt = ep.CreatedAt,
//                 UpdatedAt = ep.UpdatedAt
//             }).ToList()
//         };

//         return Result.Success(response);
//     }
// }
