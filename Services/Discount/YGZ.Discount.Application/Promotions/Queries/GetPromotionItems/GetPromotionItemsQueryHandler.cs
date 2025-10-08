
//using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
//using YGZ.BuildingBlocks.Shared.Abstractions.Result;
//using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
//using YGZ.Discount.Domain.Abstractions.Data;
//using YGZ.Discount.Domain.PromotionItem;
//using YGZ.Discount.Domain.PromotionItem.ValueObjects;

//namespace YGZ.Discount.Application.Promotions.Queries.GetPromotionItem;

//public class GetPromotionItemsQueryHandler : IQueryHandler<GetPromotionItemsQuery, List<PromotionItemResponse>>
//{
//    private readonly IGenericRepository<PromotionItem, PromotionItemId> _promotionItemRepository;
//    public GetPromotionItemsQueryHandler(IGenericRepository<PromotionItem, PromotionItemId> promotionItemRepository)
//    {
//        _promotionItemRepository = promotionItemRepository;
//    }

//    public async Task<Result<List<PromotionItemResponse>>> Handle(GetPromotionItemsQuery request, CancellationToken cancellationToken)
//    {
//        var result = await _promotionItemRepository.GetAllAsync(cancellationToken);

//        if (result is null || result.Count == 0)
//        {
//            return new List<PromotionItemResponse>();
//        }

//        List<PromotionItemResponse> response = MapToResponse(result);

//        return response;
//    }

//    private List<PromotionItemResponse> MapToResponse(List<PromotionItem> promotionItems)
//    {
//        return promotionItems.Select(item => new PromotionItemResponse
//        {
//            PromotionItemId = item.Id.Value.ToString()!,
//            Title = item.Title,
//            Description = item.Description,
//            PromotionEventType = item.PromotionEventType.ToString(),
//            DiscountState = item.DiscountState.ToString(),
//            DiscountType = item.DiscountType.ToString(),
//            DiscountValue = item.DiscountValue,
//            EndDiscountType = item.EndDiscountType.ToString(),
//            ProductNameTag = item.ProductNameTag.ToString(),
//            ValidFrom = item.ValidFrom,
//            ValidTo = item.ValidTo,
//            AvailableQuantity = item.AvailableQuantity,
//            ProductId = item.ProductId.Value.ToString(),
//            PromotionItemProductSlug = item.ProductSlug,
//            PromotionItemProductImage = item.ProductImage
//        }).ToList();
//    }
//}

