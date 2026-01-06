using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MapsterMapper;
using MediatR;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Api.Protos;
using YGZ.Catalog.Application.Inventory.Commands.CheckInsufficientStock;
using YGZ.Catalog.Application.Inventory.Queries.GetSkuById;
using YGZ.Catalog.Application.Tenants.Queries.GetTenantById;
using YGZ.Catalog.Domain.Core.Errors;

namespace YGZ.Catalog.Api.GrpcServices;

public class CatalogRpcService : CatalogProtoService.CatalogProtoServiceBase
{
    private readonly IMapper _mapper;
    private readonly ISender _sender;
    private readonly ILogger<CatalogRpcService> _logger;

    public CatalogRpcService(IMapper mapper,
                             ISender sender,
                             ILogger<CatalogRpcService> logger)
    {
        _mapper = mapper;
        _sender = sender;
        _logger = logger;
    }

    public async override Task<BooleanResponse> CheckTenantExistGrpc(CheckTenantExistRequest request, ServerCallContext context)
    {
        return await base.CheckTenantExistGrpc(request, context);
    }

    public async override Task<TenantModel> GetTenantByIdGrpc(GetTenantByIdRequest request, ServerCallContext context)
    {
        var query = new GetTenantByIdQuery
        {
            TenantId = request.TenantId
        };

        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            throw new RpcException(new Status(StatusCode.NotFound,
                                              result.Error.Message), new Metadata
            {
                { "error-code", result.Error.Code },
                { "service-name", "DiscountService" }
            });
        }

        return new TenantModel
        {
            Id = result.Response.Id,
            SubDomain = result.Response.SubDomain,
            Name = result.Response.Name,
            Description = result.Response.Description,
            TenantType = ConvertToETenantTypeGrpc(ETenantType.FromName(result.Response.TenantType)),
            TenantState = ConvertToETenantStateGrpc(ETenantState.FromName(result.Response.TenantState)),
        };
    }

    public async override Task<SkuModel> GetSkuByIdGrpc(GetSkuByIdRequest request, ServerCallContext context)
    {
        var query = new GetSkuByIdQuery(request.SkuId);

        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            if (result.Error == Errors.Inventory.SkuDoesNotExist)
            {
                throw new RpcException(new Status(StatusCode.NotFound, result.Error.Message), new Metadata
                {
                    { "error-code", result.Error.Code },
                    { "service-name", "CatalogService" }
                });
            }
            else
            {
                throw new RpcException(new Status(StatusCode.Internal, result.Error.Message), new Metadata
                {
                    { "error-code", result.Error.Code },
                    { "service-name", "CatalogService" }
                });
            }
        }

        var skuModel = new SkuModel
        {
            Id = result.Response!.Id,
            ModelId = result.Response.ModelId,
            TenantId = result.Response.TenantId,
            BranchId = result.Response.BranchId,
            SkuCode = result.Response.Code,
            ProductClassification = result.Response.ProductClassification,
            NormalizedModel = result.Response.Model.NormalizedName,
            NormalizedStorage = result.Response.Storage.NormalizedName,
            NormalizedColor = result.Response.Color.NormalizedName,
            ColorHexCode = result.Response.Color.HexCode,
            UnitPrice = (double)result.Response.UnitPrice,
            AvailableInStock = result.Response.AvailableInStock,
            TotalSold = result.Response.TotalSold,
            SkuState = ConvertToESkuStateGrpc(ESkuState.FromName(result.Response.State)),
            Slug = result.Response.Slug,
            CreatedAt = ToTimestampUtc(result.Response.CreatedAt),
            UpdatedAt = ToTimestampUtc(result.Response.UpdatedAt),
            UpdatedBy = result.Response.UpdatedBy,
            IsDeleted = result.Response.IsDeleted,
            DeletedAt = ToTimestampUtc(result.Response.DeletedAt),
            DeletedBy = result.Response.DeletedBy
        };

        if (result.Response.ReservedForEvent is not null)
        {
            skuModel.ReservedForEvent = new ReservedForEventFieldModel
            {
                EventId = result.Response.ReservedForEvent.EventId,
                EventItemId = result.Response.ReservedForEvent.EventItemId,
                EventName = result.Response.ReservedForEvent.EventName,
                ReservedQuantity = result.Response.ReservedForEvent.ReservedQuantity
            };
        }

        return skuModel;
    }

    public async override Task<BooleanResponse> CheckInsufficientGrpc(CheckInsufficientRequest request, ServerCallContext context)
    {
        var promotionType = request.PromotionType switch
        {
            EPromotionTypeGrpc.PromotionTypeCoupon => EPromotionType.COUPON,
            EPromotionTypeGrpc.PromotionTypeEventItem => EPromotionType.EVENT_ITEM,
            _ => EPromotionType.UNKNOWN
        };

        var command = new CheckInsufficientStockCommand
        {
            SkuId = request.SkuId,
            Quantity = request.Quantity.HasValue ? request.Quantity.Value : 0,
            PromotionId = request.PromotionId,
            PromotionType = promotionType
        };

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            if (result.Error == Errors.Inventory.SkuDoesNotExist)
            {
                throw new RpcException(new Status(StatusCode.NotFound, result.Error.Message), new Metadata
                {
                    { "error-code", result.Error.Code },
                    { "service-name", "CatalogService" }
                });
            }
            else
            {
                throw new RpcException(new Status(StatusCode.Internal, result.Error.Message), new Metadata
                {
                    { "error-code", result.Error.Code },
                    { "service-name", "CatalogService" }
                });
            }
        }

        return new BooleanResponse { IsSuccess = result.Response };
    }

    // privates methods
    private static ETenantTypeGrpc ConvertToETenantTypeGrpc(ETenantType tenantType)
    {
        return tenantType.Name switch
        {
            "WAREHOUSE" => ETenantTypeGrpc.TenantTypeWarehouse,
            "BRANCH" => ETenantTypeGrpc.TenantTypeBranch,
            _ => ETenantTypeGrpc.TenantTypeUnknown
        };
    }

    private static ETenantStateGrpc ConvertToETenantStateGrpc(ETenantState tenantState)
    {
        return tenantState.Name switch
        {
            "ACTIVE" => ETenantStateGrpc.TenantStateActive,
            "INACTIVE" => ETenantStateGrpc.TenantStateInactive,
            "MAINTENANCE" => ETenantStateGrpc.TenantStateMaintenance,
            _ => ETenantStateGrpc.TenantStateUnknown
        };
    }

    private static ESkuStateGrpc ConvertToESkuStateGrpc(ESkuState skuState)
    {
        return skuState.Name switch
        {
            "ACTIVE" => ESkuStateGrpc.SkuStateActive,
            "INACTIVE" => ESkuStateGrpc.SkuStateInactive,
            _ => ESkuStateGrpc.SkuStateUnknown
        };
    }

    private static Timestamp ToTimestampUtc(DateTime dateTime)
    {
        return Timestamp.FromDateTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
    }

    private static Timestamp? ToTimestampUtc(DateTime? dateTime)
    {
        return dateTime is null ? null : ToTimestampUtc(dateTime.Value);
    }
}
