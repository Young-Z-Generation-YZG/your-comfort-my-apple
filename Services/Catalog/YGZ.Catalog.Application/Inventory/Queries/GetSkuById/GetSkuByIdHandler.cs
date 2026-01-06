using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Inventory.Queries.GetSkuById;

public class GetSkuByIdHandler : IQueryHandler<GetSkuByIdQuery, SkuResponse>
{
    private readonly ILogger<GetSkuByIdHandler> _logger;
    private readonly IMongoRepository<SKU, SkuId> _repository;

    public GetSkuByIdHandler(ILogger<GetSkuByIdHandler> logger, IMongoRepository<SKU, SkuId> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<SkuResponse>> Handle(GetSkuByIdQuery request, CancellationToken cancellationToken)
    {
        var sku = await _repository.GetByIdAsync(request.SkuId, cancellationToken);

        if (sku is null)
        {
            _logger.LogError(":::[QueryHandler:{QueryHandler}][Result:Error][Method:{MethodName}:::] Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(GetSkuByIdHandler), nameof(_repository.GetByIdAsync), Errors.Inventory.SkuDoesNotExist.Message, request);

            return Errors.Inventory.SkuDoesNotExist;
        }

        return sku.ToResponse();
    }
}
