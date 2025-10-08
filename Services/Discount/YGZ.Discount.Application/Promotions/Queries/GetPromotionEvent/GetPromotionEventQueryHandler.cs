
//using MapsterMapper;
//using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
//using YGZ.BuildingBlocks.Shared.Abstractions.Result;
//using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
//using YGZ.Discount.Domain.Abstractions.Data;
//using YGZ.Discount.Domain.Core.Enums;
//using YGZ.Discount.Domain.PromotionEvent;
//using YGZ.Discount.Domain.PromotionEvent.Entities;
//using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

//namespace YGZ.Discount.Application.Promotions.Queries.GetPromotionEventQuery;

//public class GetPromotionEventQueryHandler : IQueryHandler<GetPromotionEventQuery, List<PromotionGlobalEventResponse>>
//{
//    private readonly IGenericRepository<Domain.PromotionEvent.PromotionEvent, PromotionEventId> _promotionEventRepository;
//    private readonly IGenericRepository<PromotionGlobal, PromotionGlobalId> _promotionGlobalRepository;
//    private readonly IGenericRepository<PromotionProduct, ProductId> _promotionProductRepository;
//    private readonly IGenericRepository<PromotionCategory, CategoryId> _promotionCategoryRepository;
//    private readonly IMapper _mapper;

//    public GetPromotionEventQueryHandler(IGenericRepository<PromotionGlobal, PromotionGlobalId> promotionGlobalRepository,
//                                          IGenericRepository<PromotionProduct, ProductId> promotionProductRepository,
//                                          IGenericRepository<PromotionCategory, CategoryId> promotionCategoryRepository,
//                                          IGenericRepository<Domain.PromotionEvent.PromotionEvent, PromotionEventId> promotionEventRepository,
//                                          IMapper mapper)
//    {
//        _promotionEventRepository = promotionEventRepository;
//        _promotionGlobalRepository = promotionGlobalRepository;
//        _promotionProductRepository = promotionProductRepository;
//        _promotionCategoryRepository = promotionCategoryRepository;
//        _mapper = mapper;
//    }

//    public async Task<Result<List<PromotionGlobalEventResponse>>> Handle(GetPromotionEventQuery request, CancellationToken cancellationToken)
//    {
//        var promotionEvents = await _promotionEventRepository.GetAllAsync(cancellationToken);

//        var promotionGlobals = await _promotionGlobalRepository.GetAllAsync(cancellationToken);

//        List<PromotionGlobalEventResponse> responses = new();

//        if (promotionEvents.Any())
//        {

//            foreach (var item in promotionEvents)
//            {
//                PromotionGlobalEventResponse group = new();

//                group.promotionEvent = new PromotionEventResponse
//                {
//                    PromotionEventId = item.Id is not null ? item.Id.Value.ToString() : null,
//                    PromotionEventTitle = item.Title,
//                    PromotionEventDescription = item.Description,
//                    PromotionEventType = item.PromotionEventType.Name,
//                    PromotionEventState = item.DiscountState.Name,
//                    PromotionEventValidFrom = item.ValidFrom,
//                    PromotionEventValidTo = item.ValidTo
//                };

//                promotionGlobals.Select(x => x.PromotionEventId == item.Id);


//                foreach (var promotionGlobal in promotionGlobals)
//                {
//                    if (promotionGlobal.PromotionEventId == item.Id)
//                    {
//                        List<PromotionProduct> promotionProducts = new();
//                        List<PromotionCategory> promotionCategories = new();

//                        if (promotionGlobal.PromotionGlobalType.Equals(PromotionGlobalType.PRODUCTS))
//                        {
//                            promotionProducts = await _promotionProductRepository.GetAllByFilterAsync(x => x.PromotionGlobalId == promotionGlobal.Id, cancellationToken);
//                            group.PromotionProducts = promotionProducts.Select(x => _mapper.Map<PromotionProductResponse>(x)).ToList();
//                        }

//                        if (promotionGlobal.PromotionGlobalType.Equals(PromotionGlobalType.CATEGORIES))
//                        {
//                            promotionCategories = await _promotionCategoryRepository.GetAllByFilterAsync(x => x.PromotionGlobalId == promotionGlobal.Id, cancellationToken);
//                            group.PromotionCategories = promotionCategories.Select(x => _mapper.Map<PromotionCategoryResponse>(x)).ToList();
//                        }

//                    }
//                }

//                responses.Add(group);
//            }
//        }

//        return responses;
//    }
//}
