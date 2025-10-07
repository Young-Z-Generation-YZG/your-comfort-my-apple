using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Promotions;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Catalog.Application.Promotions.Queries.GetPromotionItems;

public class GetPromotionItemsHandler : IQueryHandler<GetPromotionItemsQuery, PromotionIphoneItemEventResponse>
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly IMongoRepository<IphoneModel, ModelId> _iPhoneModelRepository;


    public GetPromotionItemsHandler(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient, IMongoRepository<IphoneModel, ModelId> iPhoneModelRepository)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
        _iPhoneModelRepository = iPhoneModelRepository;
    }

    public async Task<Result<PromotionIphoneItemEventResponse>> Handle(GetPromotionItemsQuery request, CancellationToken cancellationToken)
    {
        // example response from discount items
        var eventProductSkuId = "04edf970-b569-44ac-a116-9847731929ab";
        var iphoneModelId = "68e403d5617b27ad030bf28f";
        var categoryId = "68e23da02a7a6ccf74f12620";
        var categoryName = "iPhones";
        var model = "IPHONE_15";
        var color = "BLUE";
        var storage = "128GB";
        var originalPrice = 1300;
        var discountType = "PERCENTAGE";
        var discountValue = 0.1m;
        var stock = 10;
        var Sold = 5;
        var imageUrl = "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp";
        var modelSlug = "iphone-15";

        var iphoneModel = await _iPhoneModelRepository.GetByIdAsync(iphoneModelId, cancellationToken);

        if (iphoneModel is null)
        {
            return Errors.Iphone.ModelDoesNotExist;
        }

        var colorValueObject = iphoneModel.Colors.FirstOrDefault(x => x.NormalizedName == color);
        var storageValueObject = iphoneModel.Storages.FirstOrDefault(x => x.NormalizedName == storage);
        var modelValueObject = iphoneModel.Models.FirstOrDefault(x => x.NormalizedName == model);

        if (colorValueObject is null || storageValueObject is null || modelValueObject is null)
        {
            throw new Exception("Color, storage or model not found");
        }

        var discountAmount = CalculateDiscountAmount(originalPrice, discountValue, discountType);
        var finalPrice = originalPrice - discountAmount;

        return MapToResponse(eventProductSkuId,
                             iphoneModel,
                             modelValueObject,
                             colorValueObject,
                             storageValueObject,
                             originalPrice,
                             discountType,
                             discountValue,
                             discountAmount,
                             finalPrice,
                             stock,
                             Sold,
                             imageUrl,
                             modelSlug);
    }

    private decimal CalculateDiscountAmount(decimal originalPrice, decimal discountValue, string discountType)
    {
        if (discountType == "PERCENTAGE")
        {
            return originalPrice * discountValue;
        }
        else if (discountType == "FIXED_AMOUNT")
        {
            return discountValue;
        }

        throw new ArgumentException("Invalid discount type");
    }

    private PromotionIphoneItemEventResponse MapToResponse(
        string eventProductSkuId,
        IphoneModel iphoneModel,
        Model modelValueObject,
        Color colorValueObject,
        Storage storageValueObject,
        decimal originalPrice,
        string discountType,
        decimal discountValue,
        decimal discountAmount,
        decimal finalPrice,
        int stock,
        int Sold,
        string imageUrl,
        string modelSlug
    )
    {

        return new PromotionIphoneItemEventResponse
        {
            EventProductSkuId = eventProductSkuId,
            ModelId = iphoneModel.Id.Value!,
            Category = iphoneModel.Category.ToResponse(),
            Model = modelValueObject.ToResponse(),
            Color = colorValueObject.ToResponse(),
            Storage = storageValueObject.ToResponse(),
            OriginalPrice = originalPrice,
            DiscountType = discountType,
            DiscountValue = discountValue,
            DiscountAmount = discountAmount,
            FinalPrice = finalPrice,
            Stock = stock,
            Sold = Sold,
            ImageUrl = imageUrl,
            ModelSlug = modelSlug,
        };
    }
}

