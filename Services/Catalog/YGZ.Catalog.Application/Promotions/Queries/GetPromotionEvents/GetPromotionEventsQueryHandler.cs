


using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Domain.Core.Enums;
using DiscountProtos = YGZ.Discount.Grpc.Protos;

namespace YGZ.Catalog.Application.Promotions.Queries.GetPromotionEvents;

public class GetPromotionEventsQueryHandler : IQueryHandler<GetPromotionEventsQuery, PaginationResponse<PromotionEventResponse>>
{
    private readonly DiscountProtos.DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public GetPromotionEventsQueryHandler(DiscountProtos.DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<PaginationResponse<PromotionEventResponse>>> Handle(GetPromotionEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await _discountProtoServiceClient.GetPromotionEventAsync(new DiscountProtos.GetPromotionEventRequest());

        if (events is null)
        {
            return new PaginationResponse<PromotionEventResponse>();
        }

        PaginationResponse<PromotionEventResponse> response = MapToResponse(events, request);

        return response;
    }

    private PaginationResponse<PromotionEventResponse> MapToResponse(DiscountProtos.PromotionEventResponse events, GetPromotionEventsQuery request)
    {
        PaginationResponse<PromotionEventResponse> res = new PaginationResponse<PromotionEventResponse>();

        res.TotalRecords = 0;
        res.TotalPages = 0;
        res.PageSize = 0;
        res.CurrentPage = 0;

        var queryParams = QueryParamBuilder.Build(request);

        PaginationLinks paginationLinks = PaginationLinksBuilder.Build(basePath: "", queryParams: queryParams, currentPage: 1, totalPages: 1);

        res.Links = paginationLinks;

        res.Items = events.PromotionEvents.Select(x => new PromotionEventResponse
        {
            PromotionEventId = x.PromotionEvent.PromotionEventId,
            PromotionEventTitle = x.PromotionEvent.PromotionEventTitle,
            PromotionEventDescription = x.PromotionEvent.PromotionEventDescription,
            PromotionEventType = PromotionEventType.FromValue((int)x.PromotionEvent.PromotionEventPromotionEventType).Name,
            PromotionEventState = x.PromotionEvent.PromotionEventState.ToString().ToUpper(),
            PromotionEventValidFrom = x.PromotionEvent.PromotionEventValidFrom.ToDateTime(),
            PromotionEventValidTo = x.PromotionEvent.PromotionEventValidTo.ToDateTime()
        }).ToList();


        return res;
    }
}
