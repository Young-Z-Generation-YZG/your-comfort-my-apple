using Google.Protobuf.WellKnownTypes;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Catalog.Application.IPhone16.Queries.GetIPhonePromotions;

public class GetIPhonePromotionsQueryHandler : IQueryHandler<GetIPhonePromotionsQuery, PaginationPromotionResponse<IPhoneResponse>>
{
    private readonly IMongoRepository<IPhone16Detail> _iPhone16repository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public GetIPhonePromotionsQueryHandler(IMongoRepository<IPhone16Detail> iPhone16repository, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _iPhone16repository = iPhone16repository;
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<PaginationPromotionResponse<IPhoneResponse>>> Handle(GetIPhonePromotionsQuery request, CancellationToken cancellationToken)
    {
        var promotionEvents = await _discountProtoServiceClient.GetPromotionEventAsync(new GetPromotionEventRequest());

        var validEvent = promotionEvents.PromotionEvents.FirstOrDefault(pe => pe.PromotionEvent.PromotionEventState == DiscountStateEnum.Active);

        List<PromotionProductModel> promotionProducts = new List<PromotionProductModel>();
        List<PromotionCategoryModel> promotionCategories = new List<PromotionCategoryModel>();

        if (validEvent is not null)
        {
            if (validEvent.PromotionProducts.Count != 0)
            {
                promotionProducts = validEvent.PromotionProducts.ToList();
            }

            if (validEvent.PromotionCategories.Count != 0)
            {
                promotionCategories = validEvent.PromotionCategories.ToList();
            }
        }

        var (filter, sort) = GetFilterDefinition(request);

        var result = await _iPhone16repository.GetAllAsync(request.Page, request.Limit, filter, sort, cancellationToken);

        var promotionItems = PromotionMapping(result.items, validEvent, promotionProducts, promotionCategories);

        var response = MapToResponse(result.items, promotionItems, result.totalRecords, result.totalPages, request);

        return response;
    }

    private List<PromotionDataResponse> PromotionMapping(List<IPhone16Detail> items,
                                      ListPromtionEventResponse? promotionEvent,
                                      List<PromotionProductModel> promotionProducts,
                                      List<PromotionCategoryModel> promotionCategories)
    {
        var promotionItems = new List<PromotionDataResponse>(); 

        items.ForEach(item =>
        {
            var promotionProduct = promotionProducts.FirstOrDefault(pp => pp.PromotionProductSlug == item.Slug.Value);
            var promotionCategory = promotionCategories.FirstOrDefault(pc => pc.PromotionCategoryId == item.CategoryId.Value);

            decimal promotionPrice = item.UnitPrice; 
            decimal promotionDiscountValue = 0;     
            DiscountTypeEnum DiscountType = DiscountTypeEnum.Percentage;


            if (promotionProduct is not null)
            {
                DiscountType = promotionProduct.PromotionProductDiscountType;
                decimal productDiscountPrice = item.UnitPrice - (item.UnitPrice * (decimal)promotionProduct.PromotionProductDiscountValue);
                if (productDiscountPrice < promotionPrice)
                {
                    promotionPrice = productDiscountPrice;
                    promotionDiscountValue = (decimal)promotionProduct.PromotionProductDiscountValue;
                }
            }

            if (promotionCategory is not null)
            {
                DiscountType = promotionCategory.PromotionCategoryDiscountType;
                decimal categoryDiscountPrice = item.UnitPrice - (item.UnitPrice * (decimal)promotionCategory.PromotionCategoryDiscountValue);
                if (categoryDiscountPrice < promotionPrice)
                {
                    promotionPrice = categoryDiscountPrice;
                    promotionDiscountValue = (decimal)promotionCategory.PromotionCategoryDiscountValue;
                }
            }

            var promotionData = new PromotionDataResponse()
            {
                PromotionProductId = item.Id.Value,
                PromotionTitle = promotionEvent!.PromotionEvent.PromotionEventTitle,
                PromotionDiscountType = DiscountType.ToString(),
                PromotionDiscountValue = promotionDiscountValue,
                PromotionProductSlug = item.Slug.Value,
                PromotionEventType = promotionEvent!.PromotionEvent.PromotionEventPromotionEventType.ToString(),
                PromotionFinalPrice = promotionPrice
            };

            promotionItems.Add(promotionData);
        });

        return promotionItems;
    }

    private (FilterDefinition<IPhone16Detail> filterBuilder, SortDefinition<IPhone16Detail> sort) GetFilterDefinition(GetIPhonePromotionsQuery request)
    {
        var filterBuilder = Builders<IPhone16Detail>.Filter;
        var filter = filterBuilder.Empty;


        var sortBuilder = Builders<IPhone16Detail>.Sort;
        var sort = sortBuilder.Ascending(x => x.UnitPrice); // default sort


        return (filter, sort);
    }

    private PaginationPromotionResponse<IPhoneResponse> MapToResponse(List<IPhone16Detail> productItems, List<PromotionDataResponse> promotionDatas, int totalRecords, int totalPages, GetIPhonePromotionsQuery request)
    {
        var queryParams = QueryParamBuilder.Build(request);

        var links = PaginationLinksBuilder.Build(basePath: "/api/v1/products/iphone/promotions",
                                                 queryParams: queryParams,
                                                 currentPage: request.Page ?? 1,
                                                 totalPages: totalPages);

        var productResponses = productItems.Select(p => new IPhoneResponse
        {
            ProductId = p.Id.Value!,
            ProductModel = p.Model,
            ProductColor = new ColorResponse
            {
                ColorName = p.Color.ColorName,
                ColorHex = p.Color.ColorHex,
                ColorImage = p.Color.ColorImage,
                ColorOrder = p.Color.ColorOrder
            },
            ProductStorage = p.Storage,
            ProductUnitPrice = p.UnitPrice,
            ProductAvailableInStock = p.AvailableInStock,
            ProductDescription = p.Description,
            ProductImages = p.Images.Select(i => new ImageResponse
            {
                ImageId = i.ImageId,
                ImageName = i.ImageName,
                ImageUrl = i.ImageUrl,
                ImageDescription = i.ImageDescription,
                ImageWidth = i.ImageWidth,
                ImageHeight = i.ImageHeight,
                ImageBytes = i.ImageBytes,
                ImageOrder = i.ImageOrder
            }).ToList(),
            ProductSlug = p.Slug.Value,
        }).ToList();

        var response = new PaginationPromotionResponse<IPhoneResponse>
        {
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            PageSize = request.Limit ?? 10,
            CurrentPage = request.Page ?? 1,
            Items = productResponses,
            PromotionItems = promotionDatas,
            Links = links
        };

        return response;
    }
}
