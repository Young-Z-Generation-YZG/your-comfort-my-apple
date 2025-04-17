

using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16;
using MongoDB.Driver;
using YGZ.Catalog.Domain.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Errors;
using MapsterMapper;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.Discount.Grpc.Protos;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.WithPromotion;
using YGZ.Catalog.Domain.Core.Enums;

namespace YGZ.Catalog.Application.IPhone16.Queries.GetIPhonesByModelSlug;

public class GetIPhonesByModelSlugQueryHandler : IQueryHandler<GetIPhonesByModelSlugQuery, List<IPhoneDetailsWithPromotionResponse>>
{
    private readonly IMongoRepository<IPhone16Detail> _detailRepository;
    private readonly IMongoRepository<IPhone16Model> _modelRepository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly ILogger<GetIPhonesByModelSlugQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetIPhonesByModelSlugQueryHandler(IMongoRepository<IPhone16Detail> detailRepository, IMongoRepository<IPhone16Model> modelRepository, ILogger<GetIPhonesByModelSlugQueryHandler> logger, IMapper mapper, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
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

        var promotionEvents = await _discountProtoServiceClient.GetPromotionEventAsync(new GetPromotionEventRequest());
        var promotionItems = await _discountProtoServiceClient.GetPromotionItemsAsync(new GetPromotionItemsRequest());

        var model = await _modelRepository.GetByFilterAsync(filter, cancellationToken);

        if(model is null)
        {
            return Errors.Category.DoesNotExist;
        }

        var products = await _detailRepository.GetAllAsync(Builders<IPhone16Detail>.Filter.Eq(x => x.IPhoneModelId, model.Id), cancellationToken);
        
        List<PromotionDataResponse> allPromotionProducts = PromotionMapping(products, promotionEvents, promotionItems);


        List<IPhoneDetailsWithPromotionResponse> responses = MapToResponse(products, allPromotionProducts);

        return responses;
    }

    private List<PromotionDataResponse> PromotionMapping(List<IPhone16Detail> products, PromotionEventResponse promotionEvents, PromotionItemsRepsonse promotionItems)
    {
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

        var itemsWithPromotion = new List<PromotionDataResponse>();

        products.ForEach(item =>
        {
            var promotionProduct = promotionProducts.FirstOrDefault(pp => pp.PromotionProductSlug == item.Slug.Value);
            var promotionCategory = promotionCategories.FirstOrDefault(pc => pc.PromotionCategoryId == item.CategoryId.Value);
            var promotionItem = promotionItems.PromotionItems.FirstOrDefault(pi => pi.PromotionItemProductId == item.Id.Value);

            decimal promotionPrice = item.UnitPrice;
            decimal promotionDiscountValue = 0;
            DiscountTypeEnum DiscountType = DiscountTypeEnum.Percentage;
            var promotionTitle = validEvent!.PromotionEvent.PromotionEventTitle;
            var promotionType = validEvent!.PromotionEvent.PromotionEventPromotionEventType.ToString();
            var promotionId = validEvent.PromotionEvent.PromotionEventId;


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

            if (promotionItem is not null)
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

            var variants = products.Where(x => x.Slug.Value == item.Slug.Value).ToList();

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

    private List<IPhoneDetailsWithPromotionResponse> MapToResponse(List<IPhone16Detail> products, List<PromotionDataResponse> allPromotionProducts)
    {

        var response = products.Select(product =>
        {
            var existPromotion = allPromotionProducts.FirstOrDefault(p => p.PromotionProductId == product.Id.Value);

            IPhonePromotionResponse promotion = null;

            if(existPromotion is not null)
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
                ProductStorage = new StorageResponse() {StorageName = product.Storage.Name, StorageValue = product.Storage.Value},
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
                DeletedBy = product.DeletedBy is not null ? product.DeletedBy.Value.ToString() : null,
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