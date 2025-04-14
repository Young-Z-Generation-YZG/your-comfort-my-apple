

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Catalog.Application.Promotions.Queries.GetActivePromotionEvent;

public class GetActivePromotionEventQueryHandler : IQueryHandler<GetActivePromotionEventQuery, ActivePromotionEventResponse>
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public GetActivePromotionEventQueryHandler(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<ActivePromotionEventResponse>> Handle(GetActivePromotionEventQuery request, CancellationToken cancellationToken)
    {
        var events = await _discountProtoServiceClient.GetPromotionEventAsync(new GetPromotionEventRequest());
        var activeEvent = events.PromotionEvents.Where(pe => pe.PromotionEvent.PromotionEventState == DiscountStateEnum.Active);

        if (events is null)
        {
            return null!;
        }

        if (activeEvent.ToList().Any() && activeEvent.Count() > 1)
        {
            return null!;
        }

        var activePromotionEvent = activeEvent.FirstOrDefault();

        ActivePromotionEventResponse response = MapToResponse(activePromotionEvent);

        return response;
    }

    private ActivePromotionEventResponse MapToResponse(ListPromtionEventResponse? activePromotionEvent)
    {
        return new()
        {
            PromotionEvent = new ActivePromotionEvent
            {
                PromotionEventId = activePromotionEvent!.PromotionEvent.PromotionEventId,
                PromotionEventTitle = activePromotionEvent.PromotionEvent.PromotionEventTitle,
                PromotionEventDescription = activePromotionEvent.PromotionEvent.PromotionEventDescription,
                PromotionEventState = activePromotionEvent.PromotionEvent.PromotionEventState.ToString().ToUpper(),
                PromotionEventValidFrom = activePromotionEvent.PromotionEvent.PromotionEventValidFrom.ToDateTime(),
                PromotionEventValidTo = activePromotionEvent.PromotionEvent.PromotionEventValidTo.ToDateTime()
            }
        };
    }
}
