

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MapsterMapper;
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

        var couponModel = _mapper.Map<CouponModel>(result);

        //var test = new CouponModel
        //{
        //    Title = result.Title,
        //    Description = result.Description,
        //    Type = MapTypeEnum(result.Type.Name),
        //    State = MapStateEnum(result.State.Name),
        //    DiscountValue = result.DiscountValue,
        //    MinPurchaseAmount = result.MinPurchaseAmount,
        //    MaxDiscountAmount = result.MaxDiscountAmount,
        //    ValidFrom = result.ValidFrom.HasValue ? Timestamp.FromDateTime(result.ValidFrom.Value.ToUniversalTime()) : null,
        //    ValidTo = result.ValidTo.HasValue ? Timestamp.FromDateTime(result.ValidTo.Value.ToUniversalTime()) : null,
        //    AvailableQuantity = result.AvailableQuantity
        //};

        return new CouponResponse
        {
            Coupon = couponModel
        };
    }

    public override async Task<GetAllDiscountsResponse> GetAllDiscounts(GetAllDiscountsRequest request, ServerCallContext context)
    {
        var requestState = (int)request.State;
        var state = DiscountStateEnum.FromValue(requestState);

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
        var type = DiscountTypeEnum.FromValue(requestType);

        var coupon = Coupon.Create(_uniqueCodeGenerator.GenerateUniqueCode(),
                                   request.Coupon.Title,
                                   request.Coupon.Description,
                                   type,
                                   request.Coupon.DiscountValue,
                                   request.Coupon.MinPurchaseAmount,
                                   request.Coupon.MaxDiscountAmount,
                                   validFrom,
                                   validTo,
                                   request.Coupon.AvailableQuantity);


        var result = await _discountRepository.CreateAsync(coupon);

        return new BooleanResponse { IsSuccess = result };
    }

    public override async Task<BooleanResponse> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var validFrom = request.Coupon.ValidFrom.ToDateTime();
        var validTo = request.Coupon.ValidTo.ToDateTime();
        var requestType = (int)request.Coupon.Type;
        var requestState = (int)request.Coupon.State;
        var type = DiscountTypeEnum.FromValue(requestType);
        var state = DiscountStateEnum.FromValue(requestState);

        var coupon = Coupon.Update(code: request.Code,
                                   title: request.Coupon.Title,
                                   description: request.Coupon.Description,
                                   type: type,
                                   state: state,
                                   discountValue: request.Coupon.DiscountValue,
                                   minPurchaseAmount: request.Coupon.MinPurchaseAmount,
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

    private TypeEnum MapTypeEnum(string typeName)
    {
        return typeName switch
        {
            "PERCENT" => TypeEnum.Percent,
            "FIXED" => TypeEnum.Fixed,
            _ => TypeEnum.DiscountTypeEnumUnknown
        };
    }

    private StateEnum MapStateEnum(string stateName)
    {
        return stateName switch
        {
            "ACTIVE" => StateEnum.Active,
            "INACTIVE" => StateEnum.Inactive,
            "EXPIRED" => StateEnum.Expired,
            _ => StateEnum.DiscountStateEnumUnknown
        };
    }

}
