

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MapsterMapper;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Application.Abstractions;
using YGZ.Discount.Application.Data;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Application.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IUniqueCodeGenerator _uniqueCodeGenerator;
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public DiscountService(IDiscountRepository discountRepository, IMapper mapper, IUniqueCodeGenerator uniqueCodeGenerator)
    {
        _discountRepository = discountRepository;
        _uniqueCodeGenerator = uniqueCodeGenerator;
        _mapper = mapper;
    }

    public override async Task<CouponResponse> GetDiscountByCode(GetDiscountRequest request, ServerCallContext context)
    {
        var result = await _discountRepository.GetByCode(request.Code);

        if (result is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Discount code not found"));
        }

        CouponResponse response = MapToReponse(result);

        return response;
    }

    private CouponResponse MapToReponse(Coupon result)
    {
        var couponModel = new CouponModel
        {
            Title = result.Title,
            Description = result.Description,
            State = (StateEnum)result.State.Value, // Fixing the type conversion issue
            ProductNameTag = (NameTagEnum)result.ProductNameTag.Value, // Assuming similar conversion is needed
            Type = (TypeEnum)result.Type.Value, // Assuming similar conversion is needed
            DiscountValue = result.DiscountValue,
            MaxDiscountAmount = result.MaxDiscountAmount,
            ValidFrom = result.ValidFrom.HasValue ? Timestamp.FromDateTime(result.ValidFrom.Value.ToUniversalTime()) : null,
            ValidTo = result.ValidTo.HasValue ? Timestamp.FromDateTime(result.ValidTo.Value.ToUniversalTime()) : null,
            AvailableQuantity = result.AvailableQuantity
        };

        var response = new CouponResponse
        {
            Coupon = couponModel
        };

        return response;
    }

    public override async Task<GetAllDiscountsResponse> GetAllDiscounts(GetAllDiscountsRequest request, ServerCallContext context)
    {
        var requestState = (int)request.State;
        var state = DiscountState.FromValue(requestState);

        var result = await _discountRepository.GetAllAsync(request.Page, request.Limit, state);

        var response = new GetAllDiscountsResponse
        {
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages
        };

        response.Coupons.AddRange(result.coupons.Select(c => _mapper.Map<CouponModel>(c)));

        return response;
    }

    public override async Task<BooleanResponse> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var validFrom = request.Coupon.ValidFrom.ToDateTime();
        var validTo = request.Coupon.ValidTo.ToDateTime();
        var requestType = (int)request.Coupon.Type;
        var requestNameTag = (int)request.Coupon.ProductNameTag;

        var type = DiscountType.FromValue(requestType);
        var productNameTag = NameTag.FromValue(requestNameTag);

        var coupon = Coupon.Create(code: _uniqueCodeGenerator.GenerateUniqueCode(),
                                   title: request.Coupon.Title,
                                   description: request.Coupon.Description,
                                   type: type,
                                   discountValue: request.Coupon.DiscountValue,
                                   nameTag: productNameTag,
                                   maxDiscountAmount: request.Coupon.MaxDiscountAmount,
                                   validFrom: validFrom,
                                   validTo: validTo,
                                   availableQuantity: request.Coupon.AvailableQuantity);


        var result = await _discountRepository.CreateAsync(coupon);

        return new BooleanResponse { IsSuccess = result };
    }

    public override async Task<BooleanResponse> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var validFrom = request.Coupon.ValidFrom.ToDateTime();
        var validTo = request.Coupon.ValidTo.ToDateTime();
        var requestType = (int)request.Coupon.Type;
        var requestState = (int)request.Coupon.State;
        var requestNameTag = (int)request.Coupon.ProductNameTag;
        var type = DiscountType.FromValue(requestType);
        var state = DiscountState.FromValue(requestState);
        var productNameTag = NameTag.FromValue(requestNameTag);

        var coupon = Coupon.Update(code: request.Code,
                                   title: request.Coupon.Title,
                                   description: request.Coupon.Description,
                                   state: state,
                                   nameTag: productNameTag,
                                   type: type,
                                   discountValue: request.Coupon.DiscountValue,
                                   maxDiscountAmount: request.Coupon.MaxDiscountAmount,
                                   validFrom: validFrom,
                                   validTo: validTo,
                                   availableQuantity: request.Coupon.AvailableQuantity);

        var result = await _discountRepository.UpdateAsync(request.Code, coupon);

        return new BooleanResponse { IsSuccess = result };
    }

    public override async Task<BooleanResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var result = await _discountRepository.DeleteAsync(request.Code);

        return new BooleanResponse { IsSuccess = result };
    }
}
