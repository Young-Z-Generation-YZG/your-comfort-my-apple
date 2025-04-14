

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.PromotionItem;
using YGZ.Discount.Domain.PromotionItem.ValueObjects;

namespace YGZ.Discount.Application.Coupons.Queries.GetPromotionItemById;

public sealed class GetPromotionItemByIdQueryHandler : IQueryHandler<GetPromotionItemByIdQuery, PromotionItemResponse>
{
    private readonly IGenericRepository<PromotionItem, PromotionItemId> _repository;

    public GetPromotionItemByIdQueryHandler(IGenericRepository<PromotionItem, PromotionItemId> repository)
    {
        _repository = repository;
    }

    public async Task<Result<PromotionItemResponse>> Handle(GetPromotionItemByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(PromotionItemId.Of(request.PromotionId), cancellationToken);

        if(result is null)
        {
            return null;
        }

        PromotionItemResponse response = MapToResponse(result);

        return response;
    }

    private PromotionItemResponse MapToResponse(PromotionItem result)
    {
        return new PromotionItemResponse
        {
            PromotionItemId = result.Id.Value.ToString()!,
            Title = result.Title,
            Description = result.Description,
            PromotionEventType = result.PromotionEventType.Name,
            DiscountState = result.DiscountState.Name,
            DiscountType = result.DiscountType.Name,
            DiscountValue = result.DiscountValue,
            ProductNameTag = result.ProductNameTag.Name,
            EndDiscountType = result.EndDiscountType.Name,
            ValidFrom = result.ValidFrom,
            ValidTo = result.ValidTo,
            AvailableQuantity = result.AvailableQuantity,
            ProductId = result.ProductId.Value,
            PromotionItemProductSlug = result.ProductSlug,
            PromotionItemProductImage = result.ProductImage
        };
    }
}