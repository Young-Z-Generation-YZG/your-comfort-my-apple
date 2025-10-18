using Grpc.Core;
using MapsterMapper;
using MediatR;
using YGZ.Catalog.Api.Protos;

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
}
