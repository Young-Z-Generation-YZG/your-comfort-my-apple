// using Microsoft.EntityFrameworkCore;
// using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
// using YGZ.BuildingBlocks.Shared.Abstractions.Result;
// using YGZ.Discount.Application.Events.Commands.AddEventProductSKUs;
// using YGZ.Discount.Domain.Abstractions.Data;
// using YGZ.Discount.Domain.Core.Enums;
// using YGZ.Discount.Domain.Event;
// using YGZ.Discount.Domain.Event.Entities;
// using YGZ.Discount.Domain.Event.ValueObjects;

// namespace YGZ.Discount.Application.Events.Commands.AddEventProductSKUs;

// public class AddEventProductSKUsCommandHandler : ICommandHandler<AddEventProductSKUsCommand, bool>
// {
//     private readonly IGenericRepository<Event, EventId> _repository;

//     public AddEventProductSKUsCommandHandler(IGenericRepository<Event, EventId> repository)
//     {
//         _repository = repository;
//     }

//     public async Task<Result<bool>> Handle(AddEventProductSKUsCommand request, CancellationToken cancellationToken)
//     {
//         if (!request.EventProductSKUs.Any())
//         {
//             return Result.Failure<bool>(Error.Validation("EventProductSKUs", "At least one EventProductSKU is required"));
//         }

//         // Group by EventId to process each event separately
//         var groupedByEvent = request.EventProductSKUs.GroupBy(ep => ep.EventId);

//         foreach (var eventGroup in groupedByEvent)
//         {
//             var eventId = EventId.Of(Guid.Parse(eventGroup.Key));
            
//             // Get the event with its existing EventProductSKUs
//             var @event = await _repository.GetQueryable()
//                 .Include(e => e.EventProductSKUs)
//                 .FirstOrDefaultAsync(e => e.Id == eventId && !e.IsDeleted, cancellationToken);

//             if (@event is null)
//             {
//                 return Result.Failure<bool>(Error.NotFound("Event", eventGroup.Key));
//             }

//             // Process each EventProductSKU for this event
//             foreach (var eventProductSKUCommand in eventGroup)
//             {
//                 // Check if SKU already exists for this event
//                 var existingSKU = @event.GetEventProductSKUBySKUId(eventProductSKUCommand.SkuId);
//                 if (existingSKU is not null)
//                 {
//                     return Result.Failure<bool>(Error.Validation("SKU", $"SKU {eventProductSKUCommand.SkuId} already exists for event {eventGroup.Key}"));
//                 }

//                 // Create new EventProductSKU
//                 var eventProductSKU = EventProductSKU.Create(
//                     id: EventProducSKUId.Create(),
//                     eventId: eventId,
//                     skuId: eventProductSKUCommand.SkuId,
//                     modelId: eventProductSKUCommand.ModelId,
//                     modelName: eventProductSKUCommand.ModelName,
//                     storageName: eventProductSKUCommand.StorageName,
//                     colorHaxCode: eventProductSKUCommand.ColorHaxCode,
//                     productType: EProductType.FromName(eventProductSKUCommand.ProductType, false),
//                     discountType: EDiscountType.FromName(eventProductSKUCommand.DiscountType, false),
//                     imageUrl: eventProductSKUCommand.ImageUrl,
//                     discountValue: eventProductSKUCommand.DiscountValue,
//                     originalPrice: eventProductSKUCommand.OriginalPrice,
//                     stock: eventProductSKUCommand.Stock
//                 );

//                 // Add to event
//                 @event.AddEventProductSKU(eventProductSKU);
//             }

//             // Update the event (this will also save the new EventProductSKUs due to EF Core change tracking)
//             var updateResult = await _repository.UpdateAsync(@event, cancellationToken);
//             if (!updateResult.IsSuccess)
//             {
//                 return Result.Failure<bool>(updateResult.Error);
//             }
//         }

//         return Result.Success(true);
//     }
// }
