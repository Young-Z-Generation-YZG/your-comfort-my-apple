

using Google.Protobuf.WellKnownTypes;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.WithPromotion;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Iphone16;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Catalog.Application.IPhone16.Queries.GetIPhonesByModelSlug;

public class GetIPhonesByModelSlugQueryHandler : IQueryHandler<GetIPhonesByModelSlugQuery, List<IPhoneDetailsWithPromotionResponse>>
{
    private readonly IMongoRepository<IPhone16Detail, IPhone16Id> _detailRepository;
    private readonly IMongoRepository<IPhone16Model, IPhone16ModelId> _modelRepository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly ILogger<GetIPhonesByModelSlugQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetIPhonesByModelSlugQueryHandler(IMongoRepository<IPhone16Detail, IPhone16Id> detailRepository, IMongoRepository<IPhone16Model, IPhone16ModelId> modelRepository, ILogger<GetIPhonesByModelSlugQueryHandler> logger, IMapper mapper, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _detailRepository = detailRepository;
        _modelRepository = modelRepository;
        _logger = logger;
        _mapper = mapper;
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<List<IPhoneDetailsWithPromotionResponse>>> Handle(GetIPhonesByModelSlugQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<IPhone16Model>.Filter.Eq(x => x.Slug, Slug.Of(request.modelSlug));

        var model = await _modelRepository.GetByFilterAsync(filter, cancellationToken);

        if (model is null)
        {
            return Errors.Category.DoesNotExist;
        }

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

        var products = await _detailRepository.GetAllAsync(Builders<IPhone16Detail>.Filter.Eq(x => x.IPhoneModelId, model.Id), cancellationToken);

        List<PromotionDataResponse> allPromotionProducts = PromotionMapping(products, promotionEvent, promotionProducts, promotionCategories, promotionItems);

        List<IPhoneDetailsWithPromotionResponse> responses = MapToResponse(products, allPromotionProducts);

        return responses;
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


    private List<IPhoneDetailsWithPromotionResponse> MapToResponse(List<IPhone16Detail> products, List<PromotionDataResponse> allPromotionProducts)
    {

        var response = products.Select(product =>
        {
            var existPromotion = allPromotionProducts.FirstOrDefault(p => p.PromotionProductId == product.Id.Value);

            IPhonePromotionResponse promotion = null;

            if (existPromotion is not null)
            {
                var promotionEvent = PromotionEventType.PROMOTION_UNKNOWN.Name;
                var promotionDiscountType = DiscountType.UNKNOWN.Name;

                switch (existPromotion.PromotionEventType)
                {
                    case var eventType when eventType == PromotionEventTypeEnum.PromotionEvent.ToString():
                    promotionEvent = PromotionEventType.PROMOTION_EVENT.Name;
                    break;
                    case var eventType when eventType == PromotionEventTypeEnum.PromotionItem.ToString():
                    promotionEvent = PromotionEventType.PROMOTION_ITEM.Name;
                    break;
                    case var eventType when eventType == PromotionEventTypeEnum.PromotionCoupon.ToString():
                    promotionEvent = PromotionEventType.PROMOTION_COUPON.Name;
                    break;
                    case var eventType when eventType == PromotionEventType.PROMOTION_UNKNOWN.Name:
                    promotionEvent = PromotionEventType.PROMOTION_UNKNOWN.Name;
                    break;
                }

                switch (existPromotion.PromotionDiscountType)
                {
                    case var discountType when discountType == DiscountTypeEnum.Percentage.ToString():
                    promotionDiscountType = DiscountType.PERCENTAGE.Name;
                    break;
                    case var discountType when discountType == DiscountTypeEnum.Fixed.ToString():
                    promotionDiscountType = DiscountType.FIXED.Name;
                    break;
                    case var discountType when discountType == DiscountTypeEnum.Unknown.ToString():
                    promotionDiscountType = DiscountType.UNKNOWN.Name;
                    break;
                }

                promotion = new IPhonePromotionResponse()
                {
                    PromotionId = existPromotion.PromotionId,
                    PromotionProductId = existPromotion.PromotionProductId,
                    PromotionProductSlug = existPromotion.PromotionProductSlug,
                    PromotionTitle = existPromotion.PromotionTitle,
                    PromotionEventType = promotionEvent,
                    PromotionDiscountType = promotionDiscountType,
                    PromotionDiscountValue = existPromotion.PromotionDiscountValue,
                    PromotionFinalPrice = existPromotion.PromotionFinalPrice,
                    ProductNameTag = existPromotion.ProductNameTag,
                    CategoryId = existPromotion.CategoryId
                };
            }

            var colorResponse = _mapper.Map<ColorResponse>(product.Color);
            var imagesResponse = _mapper.Map<List<ImageResponse>>(product.Images);
            var response = new IPhoneDetailsWithPromotionResponse()
            {
                ProductId = product.Id.Value!,
                ProductModel = product.Model,
                ProductColor = colorResponse,
                ProductStorage = new StorageResponse() { StorageName = product.Storage.Name, StorageValue = product.Storage.Value },
                ProductUnitPrice = product.UnitPrice,
                ProductAvailableInStock = product.AvailableInStock,
                ProductState = product.State.ToString(),
                ProductNameTag = product.ProductNameTag.Name,
                ProductDescription = product.Description,
                Promotion = promotion,
                ProductSlug = product.Slug.Value,
                IphoneModelId = product.IPhoneModelId.Value!,
                CategoryId = product.CategoryId.Value!,
                TotalSold = product.TotalSold,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                DeletedAt = product.DeletedAt,
                DeletedBy = null,
                IsDeleted = product.IsDeleted,
                ProductImages = imagesResponse
            };

            response.ProductColor = colorResponse;
            response.ProductImages = imagesResponse;


            return response;
        }).ToList();

        return response;
    }
}