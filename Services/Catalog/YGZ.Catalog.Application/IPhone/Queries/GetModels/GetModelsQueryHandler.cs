
using Google.Protobuf.WellKnownTypes;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.WithPromotion;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone16;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Catalog.Application.IPhone.Queries.GetModels;

public class GetModelsQueryHandler : IQueryHandler<GetModelsQuery, PaginationResponse<IphoneModelWithPromotionResponse>>
{
    private readonly IMongoRepository<IPhone16Model, IPhone16ModelId> _modelRepository;
    private readonly IMongoRepository<IPhone16Detail, IPhone16Id> _iPhone16repository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly ILogger<GetModelsQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetModelsQueryHandler(IMongoRepository<IPhone16Model, IPhone16ModelId> modelRepository, ILogger<GetModelsQueryHandler> logger, IMapper mapper, IMongoRepository<IPhone16Detail, IPhone16Id> iPhone16repository, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _modelRepository = modelRepository;
        _iPhone16repository = iPhone16repository;
        _discountProtoServiceClient = discountProtoServiceClient;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<PaginationResponse<IphoneModelWithPromotionResponse>>> Handle(GetModelsQuery request, CancellationToken cancellationToken)
    {

        var promotionEvent = await HandleCheckValidPromotionEvent();

        var promotionItems = await _discountProtoServiceClient.GetPromotionItemsAsync(new GetPromotionItemsRequest());

        List<PromotionProductModel> promotionProducts = new List<PromotionProductModel>();
        List<PromotionCategoryModel> promotionCategories = new List<PromotionCategoryModel>();

        if (promotionEvent is not null)
        {
            if (promotionEvent.PromotionProducts.Count != 0)
            {
                promotionProducts = promotionEvent.PromotionProducts.ToList();
            }

            if (promotionEvent.PromotionCategories.Count != 0)
            {
                promotionCategories = promotionEvent.PromotionCategories.ToList();
            }
        }

        var (filter, sort) = GetFilterDefinition(request);
        var (filter2, sort2) = GetModelFilterDefinition(request);

        var allProducts = await _iPhone16repository.GetAllAsync(null, null, filter, sort, cancellationToken);
        var allModels = await _modelRepository.GetAllAsync(request.Page,request.Limit, filter2, null, cancellationToken);

        List<PromotionDataResponse> allPromotionProducts = PromotionMapping(allProducts.items, promotionEvent, promotionProducts, promotionCategories, promotionItems);

        PaginationResponse<IphoneModelWithPromotionResponse> response = MapToResponse(allModels.items, allProducts.items, allPromotionProducts);

        return response;
    }

    private async Task<ListPromtionEventResponse> HandleCheckValidPromotionEvent()
    {
        var result = await _discountProtoServiceClient.GetPromotionEventAsync(new GetPromotionEventRequest { });

        if (result is null || !result.PromotionEvents.Any())
        {
            return null;
        }
        var dateTimeNow = Timestamp.FromDateTime(DateTime.UtcNow);

        var promotionEvent = result.PromotionEvents
            .Where(x => x.PromotionEvent.PromotionEventState == DiscountStateEnum.Active)
            .Where(x => x.PromotionEvent.PromotionEventValidFrom <= dateTimeNow && dateTimeNow <= x.PromotionEvent.PromotionEventValidTo)
            .FirstOrDefault();

        if (promotionEvent is null)
        {
            return null;
        }

        return promotionEvent;
    }

    private PaginationResponse<IphoneModelWithPromotionResponse> MapToResponse(List<IPhone16Model> allModels, List<IPhone16Detail> allProducts, List<PromotionDataResponse> allPromotionProducts)
    {
        PaginationResponse<IphoneModelWithPromotionResponse> res = new PaginationResponse<IphoneModelWithPromotionResponse>();

        res.TotalRecords = 0;
        res.TotalPages = 0;
        res.PageSize = 0;
        res.CurrentPage = 0;

        PaginationLinks paginationLinks = new PaginationLinks("", "", "", "");
        res.Links = paginationLinks;

        foreach (var model in allModels)
        {
            ModelPromotionResponse promotion = null;
            var existPromotion = allPromotionProducts.FirstOrDefault(x => x.ProductModelId == model.Id.Value);

            if (existPromotion is not null)
            {
                promotion = new ModelPromotionResponse
                {
                    MinimumPromotionPrice = allPromotionProducts.Where(x => x.ProductModelId == model.Id.Value).Min(x => x.PromotionFinalPrice),
                    MaximumPromotionPrice = allPromotionProducts.Where(x => x.ProductModelId == model.Id.Value).Max(x => x.PromotionFinalPrice),
                    MinimumDiscountPercentage = allPromotionProducts.Where(x => x.ProductModelId == model.Id.Value).Min(x => x.PromotionDiscountValue),
                    MaximumDiscountPercentage = allPromotionProducts.Where(x => x.ProductModelId == model.Id.Value).Max(x => x.PromotionDiscountValue),
                };
            }

            var modelResponse = new IphoneModelWithPromotionResponse()
            {
                ModelId = model.Id.Value!,
                ModelName = model.Name,
                ModelItems = model.Models.Select(x => new ModelItemResponse
                {
                    ModelName = x.ModelName,
                    ModelOrder = (int)x.ModelOrder!,
                }).ToList(),
                ColorItems = model.Colors.Select(x => new ColorResponse
                {
                    ColorName = x.ColorName,
                    ColorHex = x.ColorHex,
                    ColorImage = x.ColorImage,
                    ColorOrder = x.ColorOrder,
                }).ToList(),
                StorageItems = model.Storages.Select(x => new StorageResponse
                {
                    StorageName = x.Name,
                    StorageValue = x.Value,
                }).ToList(),
                GeneralModel = model.GeneralModel,
                ModelDescription = model.Description,
                OverallSold = model.OverallSold,
                MinimunUnitPrice = allProducts.Where(x => x.IPhoneModelId.Value == model.Id.Value).Min(x => x.UnitPrice),
                MaximunUnitPrice = allProducts.Where(x => x.IPhoneModelId.Value == model.Id.Value).Max(x => x.UnitPrice),
                AverageRating = new AverageRatingResponse
                {
                    RatingAverageValue = model.AverageRating.RatingAverageValue,
                    RatingCount = model.AverageRating.RatingCount,
                },
                RatingStars = model.RatingStars.Select(s => new RatingStarResponse
                {
                    Star = s.Star,
                    Count = s.Count,
                }).ToList(),
                ModelImages = model.DescriptionImages.Select(x => new ImageResponse
                {
                    ImageId = x.ImageId,
                    ImageUrl = x.ImageUrl,
                    ImageName = x.ImageName,
                    ImageDescription = x.ImageDescription,
                    ImageWidth = x.ImageWidth,
                    ImageHeight = x.ImageHeight,
                    ImageBytes = x.ImageBytes,
                    ImageOrder = x.ImageOrder,
                }).ToList(),
                ModelPromotion = promotion,
                ModelSlug = model.Slug.Value!,
                CategoryId = model.CategoryId?.Value ?? string.Empty,
                IsDeleted = false,
                DeletedBy = null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            ((List<IphoneModelWithPromotionResponse>)res.Items).Add(modelResponse);
        }

        return res;
    }

    private List<PromotionDataResponse> PromotionMapping(List<IPhone16Detail> items,
                                      ListPromtionEventResponse? promotionEvent,
                                      List<PromotionProductModel> promotionProducts,
                                      List<PromotionCategoryModel> promotionCategories,
                                      PromotionItemsRepsonse promotionItems)
    {
        var itemsWithPromotion = new List<PromotionDataResponse>();

        handlePromotionProducts(itemsWithPromotion, items, promotionProducts, promotionEvent);
        handlePromotionCategories(itemsWithPromotion, items, promotionCategories, promotionEvent);
        handlePromotionItem(itemsWithPromotion, items, promotionItems);

        return itemsWithPromotion;
    }

    private void handlePromotionItem(List<PromotionDataResponse> itemsWithPromotion, List<IPhone16Detail> items, PromotionItemsRepsonse promotionItems)
    {
        foreach (var pi in promotionItems.PromotionItems)
        {
            var item = items.FirstOrDefault(i => i.Id.Value == pi.PromotionItemProductId);

            if (item is not null)
            {
                decimal promotionPrice = item.UnitPrice;
                decimal promotionDiscountValue = 0;

                DiscountTypeEnum discountType = pi.PromotionItemDiscountType;
                var promotionTitle = pi.PromotionItemTitle;
                var promotionType = pi.PromotionItemPromotionEventType;
                var promotionId = pi.PromotionItemId;

                decimal itemDiscountPrice = item.UnitPrice - (item.UnitPrice * (decimal)pi.PromotionItemDiscountValue!);

                if (itemDiscountPrice < promotionPrice)
                {
                    promotionPrice = itemDiscountPrice;
                    promotionDiscountValue = (decimal)pi.PromotionItemDiscountValue;
                }

                var productVariants = new List<ProductVariantResponse>();
                var variants = items.Where(x => x.Slug.Value == item.Slug.Value).ToList();

                foreach (var variant in variants)
                {
                    var index = variants.IndexOf(variant);
                    var productVariant = new ProductVariantResponse
                    {
                        ProductId = variant.Id.Value!,
                        ProductColorImage = variant.Images[0].ImageUrl,
                        ColorName = variant.Color.ColorName,
                        ColorHex = variant.Color.ColorHex,
                        ColorImage = variant.Color.ColorImage,
                        Order = index,
                    };
                    productVariants.Add(productVariant);
                }

                var promotionData = new PromotionDataResponse()
                {
                    PromotionId = promotionId,
                    PromotionProductId = item.Id.Value!,
                    ProductModelId = item.IPhoneModelId.Value!,
                    PromotionTitle = promotionTitle,
                    PromotionDiscountType = discountType.ToString(),
                    PromotionDiscountValue = promotionDiscountValue,
                    PromotionProductSlug = item.Slug.Value,
                    PromotionEventType = promotionType.ToString(),
                    PromotionFinalPrice = promotionPrice,
                    CategoryId = item.CategoryId.Value!,
                    ProductNameTag = item.ProductNameTag.Name,
                    ProductVariants = productVariants,
                };

                itemsWithPromotion.Add(promotionData);
            }
        }
    }

    private void handlePromotionCategories(List<PromotionDataResponse> itemsWithPromotion, List<IPhone16Detail> items, List<PromotionCategoryModel> promotionCategories, ListPromtionEventResponse? promotionEvent)
    {
        foreach (var pc in promotionCategories) 
        { 
            var itemsInPromotion = items.Where(x => x.CategoryId.Value == pc.PromotionCategoryId).ToList();

            foreach (var item in itemsInPromotion)
            {
                decimal promotionPrice = item.UnitPrice;
                decimal promotionDiscountValue = 0;

                DiscountTypeEnum discountType = pc.PromotionCategoryDiscountType;
                var promotionTitle = promotionEvent!.PromotionEvent.PromotionEventTitle;
                var promotionType = promotionEvent!.PromotionEvent.PromotionEventPromotionEventType.ToString();
                var promotionId = promotionEvent.PromotionEvent.PromotionEventId;

                decimal productDiscountPrice = item.UnitPrice - (item.UnitPrice * (decimal)pc.PromotionCategoryDiscountValue!);

                if (productDiscountPrice < promotionPrice)
                {
                    promotionPrice = productDiscountPrice;
                    promotionDiscountValue = (decimal)pc.PromotionCategoryDiscountValue;
                }

                var productVariants = new List<ProductVariantResponse>();
                var variants = items.Where(x => x.Slug.Value == item.Slug.Value).ToList();
                variants.ForEach(p =>
                {
                    var index = variants.IndexOf(p);
                    var productVariant = new ProductVariantResponse
                    {
                        ProductId = p.Id.Value!,
                        ProductColorImage = p.Images[0].ImageUrl,
                        ColorName = p.Color.ColorName,
                        ColorHex = p.Color.ColorHex,
                        ColorImage = p.Color.ColorImage,
                        Order = index,
                    };
                    productVariants.Add(productVariant);
                });

                var promotionData = new PromotionDataResponse()
                {
                    PromotionId = promotionId,
                    PromotionProductId = item.Id.Value!,
                    ProductModelId = item.IPhoneModelId.Value!,
                    PromotionTitle = promotionTitle,
                    PromotionDiscountType = discountType.ToString(),
                    PromotionDiscountValue = promotionDiscountValue,
                    PromotionProductSlug = item.Slug.Value,
                    PromotionEventType = promotionType,
                    PromotionFinalPrice = promotionPrice,
                    CategoryId = item.CategoryId.Value!,
                    ProductNameTag = item.ProductNameTag.Name,
                    ProductVariants = productVariants,
                };

                itemsWithPromotion.Add(promotionData);
            }
        }
    }

    private void handlePromotionProducts(List<PromotionDataResponse> itemsWithPromotion, List<IPhone16Detail> items, List<PromotionProductModel> promotionProducts, ListPromtionEventResponse? promotionEvent)
    {
        foreach (var pp in promotionProducts)
        {
            var item = items.FirstOrDefault(x => x.Slug.Value == pp.PromotionProductSlug);

            if (item is not null)
            {
                decimal promotionPrice = item.UnitPrice;
                decimal promotionDiscountValue = 0;

                DiscountTypeEnum discountType = pp.PromotionProductDiscountType;
                var promotionTitle = promotionEvent!.PromotionEvent.PromotionEventTitle;
                var promotionType = promotionEvent!.PromotionEvent.PromotionEventPromotionEventType.ToString();
                var promotionId = promotionEvent.PromotionEvent.PromotionEventId;

                decimal productDiscountPrice = item.UnitPrice - (item.UnitPrice * (decimal)pp.PromotionProductDiscountValue!);

                if (productDiscountPrice < promotionPrice)
                {
                    promotionPrice = productDiscountPrice;
                    promotionDiscountValue = (decimal)pp.PromotionProductDiscountValue;
                }

                var productVariants = new List<ProductVariantResponse>();

                var variants = items.Where(x => x.Slug.Value == item.Slug.Value).ToList();

                variants.ForEach(p =>
                {
                    var index = variants.IndexOf(p);
                    var productVariant = new ProductVariantResponse
                    {
                        ProductId = p.Id.Value!,
                        ProductColorImage = p.Images[0].ImageUrl,
                        ColorName = p.Color.ColorName,
                        ColorHex = p.Color.ColorHex,
                        ColorImage = p.Color.ColorImage,
                        Order = index,
                    };
                    productVariants.Add(productVariant);
                });

                var promotionData = new PromotionDataResponse()
                {
                    PromotionId = promotionId,
                    PromotionProductId = item.Id.Value!,
                    ProductModelId = item.IPhoneModelId.Value!,
                    PromotionTitle = promotionTitle,
                    PromotionDiscountType = discountType.ToString(),
                    PromotionDiscountValue = promotionDiscountValue,
                    PromotionProductSlug = item.Slug.Value,
                    PromotionEventType = promotionType,
                    PromotionFinalPrice = promotionPrice,
                    CategoryId = item.CategoryId.Value!,
                    ProductNameTag = item.ProductNameTag.Name,
                    ProductVariants = productVariants,
                };

                itemsWithPromotion.Add(promotionData);
            }
        }
    }

    private (FilterDefinition<IPhone16Detail> filterBuilder, SortDefinition<IPhone16Detail> sort) GetFilterDefinition(GetModelsQuery request)
    {
        var filterBuilder = Builders<IPhone16Detail>.Filter;
        var filter = filterBuilder.Empty;

        if (request.ProductColors is not null && request.ProductColors.Any())
        {
            var productColors = request.ProductColors.ToList();

            filter &= filterBuilder.In(x => x.Color.ColorName, productColors);
        }

        if (request.ProductStorages is not null && request.ProductStorages.Any())
        {
            var productStorages = request.ProductStorages.ToList();

            filter &= filterBuilder.In(x => x.Storage.Name, productStorages);
        }

        if (request.ProductModels is not null && request.ProductModels.Any())
        {
            var productModels = request.ProductModels.Select(x => x.ToLower()).ToList();
            filter &= filterBuilder.In(x => x.GeneralModel, productModels);
        }

        if (!string.IsNullOrEmpty(request.PriceFrom))
        {
            decimal.TryParse(request.PriceFrom, out var priceFromValue);
            if (priceFromValue > 0)
            {
                filter &= filterBuilder.Gte(x => x.UnitPrice, priceFromValue);
            }
        }

        if (!string.IsNullOrEmpty(request.PriceTo))
        {
            decimal.TryParse(request.PriceTo, out var priceToValue);
            if (priceToValue > 0)
            {
                filter &= filterBuilder.Lte(x => x.UnitPrice, priceToValue);
            }
        }

        var sortBuilder = Builders<IPhone16Detail>.Sort;
        var sort = sortBuilder.Ascending(x => x.UnitPrice); // default sort

        if (!string.IsNullOrEmpty(request.PriceSort))
        {
            sort = request.PriceSort.ToLower() switch
            {
                "asc" => sortBuilder.Ascending(x => x.UnitPrice),
                "desc" => sortBuilder.Descending(x => x.UnitPrice),
                _ => sortBuilder.Ascending(x => x.UnitPrice),
            };
        }

        return (filter, sort);
    }

    private (FilterDefinition<IPhone16Model> filterBuilder, SortDefinition<IPhone16Model> sort) GetModelFilterDefinition(GetModelsQuery request)
    {
        var filterBuilder = Builders<IPhone16Model>.Filter;
        var filter = filterBuilder.Empty;

        if (request.ProductColors is not null && request.ProductColors.Any())
        {
            var productColors = request.ProductColors.ToList();

            filter &= filterBuilder.ElemMatch(x => x.Colors, color => productColors.Contains(color.ColorName));
        }

        if (request.ProductStorages is not null && request.ProductStorages.Any())
        {
            var productStorages = request.ProductStorages.ToList();

            filter &= filterBuilder.ElemMatch(x => x.Storages, storage => productStorages.Contains(storage.Name));
        }

        if (request.ProductModels is not null && request.ProductModels.Any())
        {
            var productModels = request.ProductModels.Select(x => x.ToLower()).ToList();

            filter &= filterBuilder.In(x => x.GeneralModel, productModels);
        }

        var sortBuilder = Builders<IPhone16Model>.Sort;
        var sort = sortBuilder.Ascending(x => x.OverallSold); // default sort


        return (filter, sort);
    }
}