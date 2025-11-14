using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Promotions;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone;
using YGZ.Discount.Grpc.Protos;
using EventItemResponse = YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Promotions.EventItemResponse;

namespace YGZ.Catalog.Application.Promotions.Queries.GetPromotionItems;

public class GetEventWithItemsHandler : IQueryHandler<GetEventWithItemsQuery, EventWithItemsResponse>
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly IMongoRepository<IphoneModel, ModelId> _iPhoneModelRepository;


    public GetEventWithItemsHandler(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient, IMongoRepository<IphoneModel, ModelId> iPhoneModelRepository)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
        _iPhoneModelRepository = iPhoneModelRepository;
    }

    public async Task<Result<EventWithItemsResponse>> Handle(GetEventWithItemsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        //// Call gRPC service to get event with event items
        //var grpcRequest = new GetEventWithEventItemsRequest();
        //var grpcResponse = await _discountProtoServiceClient.GetEventWithEventItemsGrpcAsync(grpcRequest, cancellationToken: cancellationToken);

        //if (grpcResponse?.Event == null || grpcResponse.EventItems.Count == 0)
        //{
        //    return Errors.Iphone.ModelDoesNotExist;
        //}

        //var eventItemResponses = new List<EventItemResponse>();

        //// Process each event item
        //foreach (var eventItem in grpcResponse.EventItems)
        //{
        //    var modelName = eventItem.Model?.Name;

        //    if (string.IsNullOrWhiteSpace(modelName))
        //    {
        //        continue;
        //    }

        //    // Get the corresponding iPhone model - need to search by model name, not ID
        //    var iphoneModels = await _iPhoneModelRepository.GetAllAsync();
        //    var iphoneModel = iphoneModels.FirstOrDefault(m => m.Name.Contains(modelName, StringComparison.OrdinalIgnoreCase));

        //    if (iphoneModel is null)
        //    {
        //        continue;
        //    }

        //    // Find matching value objects
        //    var modelNormalized = eventItem.Model?.NormalizedName;
        //    var colorNormalized = eventItem.Color?.NormalizedName;
        //    var storageNormalized = eventItem.Storage?.NormalizedName;

        //    var modelValueObject = iphoneModel.Models.FirstOrDefault(x => x.NormalizedName == modelNormalized);
        //    var colorValueObject = iphoneModel.Colors.FirstOrDefault(x => x.NormalizedName == colorNormalized);
        //    var storageValueObject = iphoneModel.Storages.FirstOrDefault(x => x.NormalizedName == storageNormalized);

        //    if (colorValueObject is null || storageValueObject is null || modelValueObject is null)
        //    {
        //        continue;
        //    }

        //    var originalPrice = (decimal)(eventItem.OriginalPrice ?? 0);
        //    var discountValue = (decimal)(eventItem.DiscountValue ?? 0);
        //    var discountType = ConvertGrpcEnumToNormalEnum.ConvertToEDiscountType(eventItem.DiscountType.ToString());

        //    var discountAmount = CalculateDiscountAmount(originalPrice, discountValue, discountType);
        //    var finalPrice = originalPrice - discountAmount;

        //    var itemResponse = MapToResponse(
        //        eventItemId: eventItem.Id,
        //        iphoneModel: iphoneModel,
        //        modelValueObject: modelValueObject,
        //        colorValueObject: colorValueObject,
        //        storageValueObject: storageValueObject,
        //        originalPrice: originalPrice,
        //        discountType: discountType.Name,
        //        discountValue: discountValue,
        //        discountAmount: discountAmount,
        //        finalPrice: finalPrice,
        //        stock: eventItem.Stock ?? 0,
        //        Sold: eventItem.Sold ?? 0, // Will be available after proto regeneration
        //        imageUrl: eventItem.DisplayImageUrl ?? string.Empty,
        //        modelSlug: iphoneModel.Slug.Value ?? string.Empty
        //    );

        //    eventItemResponses.Add(itemResponse);
        //}

        //if (!eventItemResponses.Any())
        //{
        //    return Errors.Iphone.ModelDoesNotExist;
        //}

        //// Create event response from gRPC data
        //var eventResponse = new EventResponse
        //{
        //    Id = grpcResponse.Event.Id ?? string.Empty,
        //    Title = grpcResponse.Event.Title ?? string.Empty,
        //    Description = grpcResponse.Event.Description ?? string.Empty,
        //    StartDate = grpcResponse.Event.StartDate?.ToDateTime(),
        //    EndDate = grpcResponse.Event.EndDate?.ToDateTime()
        //};

        //// Return the complete response with event and event items
        //return new EventWithItemsResponse
        //{
        //    Event = eventResponse,
        //    EventItems = eventItemResponses
        //};
    }

    private decimal CalculateDiscountAmount(decimal originalPrice, decimal discountValue, EDiscountType discountType)
    {
        if (discountType == EDiscountType.PERCENTAGE)
        {
            return originalPrice * discountValue;
        }
        else if (discountType == EDiscountType.FIXED_AMOUNT)
        {
            return discountValue;
        }

        throw new ArgumentException("Invalid discount type");
    }

    private EventItemResponse MapToResponse(
        string eventItemId,
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
        return new EventItemResponse
        {
            Id = eventItemId,
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
            ModelSlug = modelSlug
        };
    }
}

