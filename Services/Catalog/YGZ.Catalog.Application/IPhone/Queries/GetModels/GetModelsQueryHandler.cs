

using MapsterMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.WithPromotion;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone16;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Catalog.Application.IPhone.Queries.GetModels;

public class GetModelsQueryHandler : IQueryHandler<GetModelsQuery, PaginationResponse<IphoneModelWithPromotionResponse>>
{
    private readonly IMongoRepository<IPhone16Model> _modelRepository;
    private readonly IMongoRepository<IPhone16Detail> _iPhone16repository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly ILogger<GetModelsQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetModelsQueryHandler(IMongoRepository<IPhone16Model> modelRepository, ILogger<GetModelsQueryHandler> logger, IMapper mapper, IMongoRepository<IPhone16Detail> iPhone16repository, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _modelRepository = modelRepository;
        _iPhone16repository = iPhone16repository;
        _discountProtoServiceClient = discountProtoServiceClient;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<PaginationResponse<IphoneModelWithPromotionResponse>>> Handle(GetModelsQuery request, CancellationToken cancellationToken)
    {
        var (filter, sort) = GetFilterDefinition(request);

        var promotionEvents = await _discountProtoServiceClient.GetPromotionEventAsync(new GetPromotionEventRequest());
        var promotionItems = await _discountProtoServiceClient.GetPromotionItemsAsync(new GetPromotionItemsRequest());
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

        var allProducts = await _iPhone16repository.GetAllAsync(null, null, filter, sort, cancellationToken);
        var allModels = await _modelRepository.GetAllAsync(request.Page,request.Limit, null, null, cancellationToken);

        List<PromotionDataResponse> allPromotionProducts = PromotionMapping(allProducts.items, validEvent, promotionProducts, promotionCategories, promotionItems);

        PaginationResponse<IphoneModelWithPromotionResponse> response = MapToResponse(allModels.items, allProducts.items, allPromotionProducts);

        return response;
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
            decimal minimunPrice = allProducts.Where(x => x.IPhoneModelId.Value == model.Id.Value).Min(x => x.UnitPrice);
            decimal maximumPrice = allProducts.Where(x => x.IPhoneModelId.Value == model.Id.Value).Max(x => x.UnitPrice);
            decimal minimunPromotionPrice = allPromotionProducts.Where(x => x.ProductModelId == model.Id.Value).Min(x => x.PromotionFinalPrice);
            decimal maximumPromotionPrice = allPromotionProducts.Where(x => x.ProductModelId == model.Id.Value).Max(x => x.PromotionFinalPrice);

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
                MinimunUnitPrice = minimunPrice,
                MaximunUnitPrice = maximumPrice,
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
                ModelPromotion = new ModelPromotionResponse
                {
                    MinimumPromotionPrice = minimunPromotionPrice,
                    MaximumPromotionPrice = maximumPromotionPrice,
                    MinimumDiscountPercentage = allPromotionProducts.Min(x => x.PromotionDiscountValue),
                    MaximumDiscountPercentage = allPromotionProducts.Max(x => x.PromotionDiscountValue),
                },
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

        items.ForEach(item =>
        {
            var promotionProduct = promotionProducts.FirstOrDefault(pp => pp.PromotionProductSlug == item.Slug.Value);
            var promotionCategory = promotionCategories.FirstOrDefault(pc => pc.PromotionCategoryId == item.CategoryId.Value);
            var promotionItem = promotionItems.PromotionItems.FirstOrDefault(pi => pi.PromotionItemProductId == item.Id.Value);

            decimal promotionPrice = item.UnitPrice;
            decimal promotionDiscountValue = 0;
            DiscountTypeEnum DiscountType = DiscountTypeEnum.Percentage;
            var promotionTitle = promotionEvent!.PromotionEvent.PromotionEventTitle;
            var promotionType = promotionEvent!.PromotionEvent.PromotionEventPromotionEventType.ToString();
            var promotionId = promotionEvent.PromotionEvent.PromotionEventId;


            if (promotionProduct is not null)
            {
                DiscountType = promotionProduct.PromotionProductDiscountType;
                decimal productDiscountPrice = item.UnitPrice - (item.UnitPrice * (decimal)promotionProduct.PromotionProductDiscountValue!);
                if (productDiscountPrice < promotionPrice)
                {
                    promotionPrice = productDiscountPrice;
                    promotionDiscountValue = (decimal)promotionProduct.PromotionProductDiscountValue;
                }
            }

            if (promotionCategory is not null)
            {
                DiscountType = promotionCategory.PromotionCategoryDiscountType;
                decimal categoryDiscountPrice = item.UnitPrice - (item.UnitPrice * (decimal)promotionCategory.PromotionCategoryDiscountValue!);
                if (categoryDiscountPrice < promotionPrice)
                {
                    promotionPrice = categoryDiscountPrice;
                    promotionDiscountValue = (decimal)promotionCategory.PromotionCategoryDiscountValue;
                }
            }

            if(promotionItem is not null)
            {
                DiscountType = promotionItem.PromotionItemDiscountType;
                decimal itemDiscountPrice = item.UnitPrice - (item.UnitPrice * (decimal)promotionItem.PromotionItemDiscountValue!);
                if (itemDiscountPrice < promotionPrice)
                {
                    promotionPrice = itemDiscountPrice;
                    promotionDiscountValue = (decimal)promotionItem.PromotionItemDiscountValue;
                    promotionTitle = promotionItem.PromotionItemTitle;
                    promotionType = promotionItem.PromotionItemPromotionEventType.ToString();
                    promotionId = promotionItem.PromotionItemId;
                }
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
                PromotionDiscountType = DiscountType.ToString(),
                PromotionDiscountValue = promotionDiscountValue,
                PromotionProductSlug = item.Slug.Value,
                PromotionEventType = promotionType,
                PromotionFinalPrice = promotionPrice,
                CategoryId = item.CategoryId.Value!,
                ProductNameTag = item.ProductNameTag.Name,
                ProductVariants = productVariants,
            };

            itemsWithPromotion.Add(promotionData);
        });

        return itemsWithPromotion;
    }

    private (FilterDefinition<IPhone16Detail> filterBuilder, SortDefinition<IPhone16Detail> sort) GetFilterDefinition(GetModelsQuery request)
    {
        var filterBuilder = Builders<IPhone16Detail>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrEmpty(request.ProductColor))
        {
            filter &= filterBuilder.Eq(x => x.Color.ColorName, request.ProductColor);
        }

        if (!string.IsNullOrEmpty(request.ProductStorage))
        {
            int.TryParse(request.ProductStorage, out var storageValue);

            if (storageValue > 0)
            {
                filter &= filterBuilder.Eq(x => x.Storage.Value, storageValue);
            }
        }

        if (!string.IsNullOrEmpty(request.ProductModel))
        {
            filter &= filterBuilder.Eq(x => x.GeneralModel, request.ProductModel);
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
}