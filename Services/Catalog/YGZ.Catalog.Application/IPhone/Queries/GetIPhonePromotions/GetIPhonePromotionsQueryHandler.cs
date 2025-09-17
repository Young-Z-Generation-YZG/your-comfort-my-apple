using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.Catalog.Application.IPhone16.Queries.GetIPhonePromotions;

public class GetIPhonePromotionsQueryHandler : IQueryHandler<GetIPhonePromotionsQuery, PaginationPromotionResponse<PromotionIphoneResponse>>
{
    //private readonly IMongoRepository<IPhone16Detail, IPhone16Id> _iPhone16repository;
    //private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    //public GetIPhonePromotionsQueryHandler(IMongoRepository<IPhone16Detail, IPhone16Id> iPhone16repository, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    //{
    //    _iPhone16repository = iPhone16repository;
    //    _discountProtoServiceClient = discountProtoServiceClient;
    //}

    //public async Task<Result<PaginationPromotionResponse<PromotionIphoneResponse>>> Handle(GetIPhonePromotionsQuery request, CancellationToken cancellationToken)
    //{
    //    var promotionEvents = await _discountProtoServiceClient.GetPromotionEventAsync(new GetPromotionEventRequest());

    //    var validEvent = promotionEvents.PromotionEvents.FirstOrDefault(pe => pe.PromotionEvent.PromotionEventState == DiscountStateEnum.Active);

    //    List<PromotionProductModel> promotionProducts = new List<PromotionProductModel>();
    //    List<PromotionCategoryModel> promotionCategories = new List<PromotionCategoryModel>();

    //    if (validEvent is not null)
    //    {
    //        if (validEvent.PromotionProducts.Count != 0)
    //        {
    //            promotionProducts = validEvent.PromotionProducts.ToList();
    //        }

    //        if (validEvent.PromotionCategories.Count != 0)
    //        {
    //            promotionCategories = validEvent.PromotionCategories.ToList();
    //        }
    //    }

    //    var allProducts = await _iPhone16repository.GetAllAsync();

    //    var allPromotionProducts = PromotionMapping(allProducts, validEvent, promotionProducts, promotionCategories);

    //    var distinctPromotionProducts = allPromotionProducts
    //        .GroupBy(pp => pp.PromotionProductSlug)
    //        .Select(g => g.First())
    //        .ToList();


    //    var promotionIphoneResponses = distinctPromotionProducts.Select(pp =>
    //    {
    //        var image = allProducts.FirstOrDefault(originalProduct => originalProduct.Slug.Value == pp.PromotionProductSlug)!.Images[0];
    //        var unitPrice = allProducts.FirstOrDefault(originalProduct => originalProduct.Slug.Value == pp.PromotionProductSlug)!.UnitPrice;
    //        var eventType = pp.PromotionEventType == PromotionEventTypeEnum.PromotionEvent.ToString() ? PromotionEventType.PROMOTION_EVENT.Name : PromotionEventType.PROMOTION_UNKNOWN.Name;
    //        var discountType = pp.PromotionDiscountType == DiscountTypeEnum.Percentage.ToString() ? DiscountType.PERCENTAGE.Name : DiscountType.UNKNOWN.Name;
    //        var discountValue = !discountType.Equals(PromotionEventType.PROMOTION_UNKNOWN) ? pp.PromotionDiscountValue : 0;
    //        var finalPrice = !discountType.Equals(DiscountType.UNKNOWN) ? pp.PromotionFinalPrice : 0;

    //        return new PromotionIphoneResponse
    //        {
    //            PromotionProductName = pp.PromotionProductSlug.Replace("-", " "),
    //            PromotionProductDescription = "",
    //            PromotionProductImage = image.ImageUrl,
    //            PromotionProductUnitPrice = unitPrice,
    //            PromotionId = validEvent!.PromotionEvent.PromotionEventId,
    //            PromotionTitle = pp.PromotionTitle!,
    //            PromotionEventType = eventType,
    //            PromotionDiscountType = discountType,
    //            PromotionDiscountValue = discountValue,
    //            PromotionFinalPrice = finalPrice,
    //            PromotionProductSlug = pp.PromotionProductSlug,
    //            CategoryId = pp.CategoryId,
    //            ProductModelId = pp.ProductModelId!,
    //            ProductVariants = pp.ProductVariants,
    //            ProductNameTag = pp.ProductNameTag,
    //        };
    //    });

    //    var paginationResponses = promotionIphoneResponses.Skip((int)((request.Page - 1) * request.Limit)!).Take(request.Limit ?? 9).ToList();

    //    var totalPages = TotalPagesPagination(promotionIphoneResponses.Count(), request.Limit ?? 10);

    //    var response = MapToResponse(paginationResponses, promotionIphoneResponses.Count(), totalPages, request);

    //    return response;
    //}

    //private List<PromotionDataResponse> PromotionMapping(List<IPhone16Detail> items,
    //                                  ListPromtionEventResponse? promotionEvent,
    //                                  List<PromotionProductModel> promotionProducts,
    //                                  List<PromotionCategoryModel> promotionCategories)
    //{
    //    var promotionItems = new List<PromotionDataResponse>();

    //    items.ForEach(item =>
    //    {
    //        var promotionProduct = promotionProducts.FirstOrDefault(pp => pp.PromotionProductSlug == item.Slug.Value);
    //        var promotionCategory = promotionCategories.FirstOrDefault(pc => pc.PromotionCategoryId == item.CategoryId.Value);

    //        decimal promotionPrice = item.UnitPrice;
    //        decimal promotionDiscountValue = 0;
    //        DiscountTypeEnum DiscountType = DiscountTypeEnum.Percentage;


    //        if (promotionProduct is not null)
    //        {
    //            DiscountType = promotionProduct.PromotionProductDiscountType;
    //            decimal productDiscountPrice = item.UnitPrice - (item.UnitPrice * (decimal)promotionProduct.PromotionProductDiscountValue!);
    //            if (productDiscountPrice < promotionPrice)
    //            {
    //                promotionPrice = productDiscountPrice;
    //                promotionDiscountValue = (decimal)promotionProduct.PromotionProductDiscountValue;
    //            }
    //        }

    //        if (promotionCategory is not null)
    //        {
    //            DiscountType = promotionCategory.PromotionCategoryDiscountType;
    //            decimal categoryDiscountPrice = item.UnitPrice - (item.UnitPrice * (decimal)promotionCategory.PromotionCategoryDiscountValue!);
    //            if (categoryDiscountPrice < promotionPrice)
    //            {
    //                promotionPrice = categoryDiscountPrice;
    //                promotionDiscountValue = (decimal)promotionCategory.PromotionCategoryDiscountValue;
    //            }
    //        }

    //        var productVariants = new List<ProductVariantResponse>();

    //        var variants = items.Where(x => x.Slug.Value == item.Slug.Value).ToList();

    //        variants.ForEach(p =>
    //        {
    //            var index = variants.IndexOf(p);
    //            var productVariant = new ProductVariantResponse
    //            {
    //                ProductId = p.Id.Value!,
    //                ProductColorImage = p.Images[0].ImageUrl,
    //                ColorName = p.Color.ColorName,
    //                ColorHex = p.Color.ColorHex,
    //                ColorImage = p.Color.ColorImage,
    //                Order = index,
    //            };

    //            productVariants.Add(productVariant);
    //        });

    //        var promotionData = new PromotionDataResponse()
    //        {
    //            PromotionId = promotionEvent!.PromotionEvent.PromotionEventId,
    //            PromotionProductId = item.Id.Value!,
    //            PromotionTitle = promotionEvent!.PromotionEvent.PromotionEventTitle,
    //            PromotionDiscountType = DiscountType.ToString(),
    //            PromotionDiscountValue = promotionDiscountValue,
    //            PromotionProductSlug = item.Slug.Value,
    //            PromotionEventType = promotionEvent!.PromotionEvent.PromotionEventPromotionEventType.ToString(),
    //            PromotionFinalPrice = promotionPrice,
    //            CategoryId = item.CategoryId.Value!,
    //            ProductModelId = item.IPhoneModelId.Value,
    //            ProductNameTag = item.ProductNameTag.Name,
    //            ProductVariants = productVariants,
    //        };

    //        promotionItems.Add(promotionData);
    //    });

    //    return promotionItems;
    //}

    //private (FilterDefinition<IPhone16Detail> filterBuilder, SortDefinition<IPhone16Detail> sort) GetFilterDefinition(GetIPhonePromotionsQuery request)
    //{
    //    var filterBuilder = Builders<IPhone16Detail>.Filter;
    //    var filter = filterBuilder.Empty;


    //    var sortBuilder = Builders<IPhone16Detail>.Sort;
    //    var sort = sortBuilder.Ascending(x => x.UnitPrice); // default sort


    //    return (filter, sort);
    //}

    //private PaginationPromotionResponse<PromotionIphoneResponse> MapToResponse(List<PromotionIphoneResponse> productItems, int totalRecords, int totalPages, GetIPhonePromotionsQuery request)
    //{
    //    var queryParams = QueryParamBuilder.Build(request);

    //    var links = PaginationLinksBuilder.Build(basePath: "/api/v1/products/iphone/promotions",
    //                                             queryParams: queryParams,
    //                                             currentPage: request.Page ?? 1,
    //                                             totalPages: totalPages);

    //    var response = new PaginationPromotionResponse<PromotionIphoneResponse>
    //    {
    //        TotalRecords = totalRecords,
    //        TotalPages = totalPages,
    //        PageSize = request.Limit ?? 9,
    //        CurrentPage = request.Page ?? 1,
    //        Items = productItems,
    //        Links = links
    //    };

    //    return response;
    //}

    //private int TotalPagesPagination(int totalRecords, int limit)
    //{
    //    var totalPages = (int)Math.Ceiling((double)totalRecords / limit);

    //    return (totalPages);
    //}
    public Task<Result<PaginationPromotionResponse<PromotionIphoneResponse>>> Handle(GetIPhonePromotionsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
