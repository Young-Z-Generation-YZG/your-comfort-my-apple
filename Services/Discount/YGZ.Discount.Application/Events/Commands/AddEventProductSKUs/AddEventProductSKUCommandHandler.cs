// using Microsoft.EntityFrameworkCore;
// using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
// using YGZ.BuildingBlocks.Shared.Abstractions.Result;
// using YGZ.Discount.Application.Events.Commands.AddEventProductSKU;
// using YGZ.Discount.Domain.Abstractions.Data;
// using YGZ.Discount.Domain.Core.Enums;
// using YGZ.Discount.Domain.Event;
// using YGZ.Discount.Domain.Event.Entities;
// using YGZ.Discount.Domain.Event.ValueObjects;

// namespace YGZ.Discount.Application.Events.Commands.AddEventProductSKU;

// public class AddEventProductSKUCommandHandler : ICommandHandler<AddEventProductSKUCommand, bool>
// {
//     private readonly IGenericRepository<Event, EventId> _repository;

//     public AddEventProductSKUCommandHandler(IGenericRepository<Event, EventId> repository)
//     {
//         _repository = repository;
//     }

//     public async Task<Result<bool>> Handle(AddEventProductSKUCommand request, CancellationToken cancellationToken)
//     {
//         // Get the event with its EventProductSKUs
//         var @event = await _repository.GetQueryable()
//             .Include(e => e.EventProductSKUs)
//             .FirstOrDefaultAsync(e => e.Id == request.EventId && !e.IsDeleted, cancellationToken);

//         if (@event is null)
//         {
//             return Result.Failure<bool>(Error.NotFound("Event", request.EventId.Value));
//         }

//         // Check if SKU already exists for this event
//         var existingSKU = @event.GetEventProductSKUBySKUId(request.SKUId);
//         if (existingSKU is not null)
//         {
//             return Result.Failure<bool>(Error.Validation("SKU", "SKU already exists for this event"));
//         }

//         // Create new EventProductSKU
//         var eventProductSKU = EventProductSKU.Create(
//             id: EventProducSKUId.Create(),
//             eventId: request.EventId,
//             skuId: request.SKUId,
//             productType: request.ProductType,
//             availableQuantity: request.AvailableQuantity,
//             discountType: request.DiscountType,
//             discountValue: request.DiscountValue,
//             imageUrl: request.ImageUrl,
//             slug: request.Slug
//         );

//         // Add to event
//         @event.AddEventProductSKU(eventProductSKU);

//         // Update the event (this will also save the new EventProductSKU due to EF Core change tracking)
//         var result = await _repository.UpdateAsync(@event, cancellationToken);

//         return result.Response;
//     }
// }
