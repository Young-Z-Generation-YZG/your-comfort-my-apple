
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Catalog.Application.Promotions.Queries.GetPromotionItems;

public class GetPromotionItemsQueryHandler : IQueryHandler<GetPromotionItemsQuery, PaginationResponse<PromotionItemResponse>>
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public GetPromotionItemsQueryHandler(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<PaginationResponse<PromotionItemResponse>>> Handle(GetPromotionItemsQuery request, CancellationToken cancellationToken)
    {
        var promotionItems = await _discountProtoServiceClient.GetPromotionItemsAsync(new GetPromotionItemsRequest());

        if (promotionItems is null || promotionItems.PromotionItems.Count == 0) 
        {
            return new PaginationResponse<PromotionItemResponse>();
        }

        PaginationResponse<PromotionItemResponse> res = MapToResponse(promotionItems, request);

        return res;
    }

    private PaginationResponse<PromotionItemResponse> MapToResponse(PromotionItemsRepsonse promotionItems, GetPromotionItemsQuery request)
    {
        PaginationResponse<PromotionItemResponse> res = new PaginationResponse<PromotionItemResponse>();

        res.TotalRecords = 0;
        res.TotalPages = 0;
        res.PageSize = 0;
        res.CurrentPage = 0;

        var queryParams = QueryParamBuilder.Build(request);

        PaginationLinks paginationLinks = PaginationLinksBuilder.Build(basePath: "", queryParams: queryParams, currentPage: 1, totalPages: 1);
        res.Links = paginationLinks;

        res.Items = promotionItems.PromotionItems.Select(x => new PromotionItemResponse
        {
            PromotionItemId = x.PromotionItemId,
            Title = x.PromotionItemTitle,
            Description = x.PromotionItemDescription,
            PromotionEventType = PromotionEventType.FromValue((int)x.PromotionItemPromotionEventType).Name,
            DiscountState = DiscountType.FromValue((int)x.PromotionItemDiscountState).Name,
            DiscountType = DiscountState.FromValue((int)x.PromotionItemDiscountType).Name,
            DiscountValue = (decimal)x.PromotionItemDiscountValue!,
            EndDiscountType = EndDiscountType.FromValue((int)x.PromotionItemEndDiscountType).Name,
            ProductNameTag = ProductNameTag.FromValue((int)x.PromotionItemNameTag).Name,
            ValidFrom = x.PromotionItemValidFrom.ToDateTime(),
            ValidTo = x.PromotionItemValidTo.ToDateTime(),
            AvailableQuantity = x.PromotionItemAvailableQuantity,
            ProductId = x.PromotionItemProductId,
            PromotionItemProductSlug = x.PromotionItemProductSlug,
            PromotionItemProductImage = x.PromotionItemProductImage
        }).ToList();

        return res;
    }
}