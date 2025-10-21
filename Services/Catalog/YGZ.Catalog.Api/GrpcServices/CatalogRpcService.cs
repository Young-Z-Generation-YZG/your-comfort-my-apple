using Grpc.Core;
using MapsterMapper;
using MediatR;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Api.Protos;
using YGZ.Catalog.Application.Tenants.Queries.GetTenantById;
using YGZ.Catalog.Application.Inventory.Queries.GetSkuById;

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
        var query = new GetTenantByIdQuery(request.TenantId);

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
            Code = result.Response.Code,
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
            throw new RpcException(new Status(StatusCode.NotFound, result.Error.Message), new Metadata
            {
                { "error-code", result.Error.Code },
                { "service-name", "CatalogService" }
            });
        }

        return new SkuModel
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
            UnitPrice = (double)result.Response.UnitPrice,
            AvailableInStock = result.Response.AvailableInStock,
            TotalSold = result.Response.TotalSold,
            SkuState = ConvertToESkuStateGrpc(ESkuState.FromName(result.Response.State)),
            Slug = result.Response.Slug,
        };
    }


    // privates methods
    private static ETenantTypeGrpc ConvertToETenantTypeGrpc(ETenantType tenantType)
    {


        return tenantType.Name switch
        {
            "WARE_HOUSE" => ETenantTypeGrpc.TenantTypeWareHouse,
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
}
