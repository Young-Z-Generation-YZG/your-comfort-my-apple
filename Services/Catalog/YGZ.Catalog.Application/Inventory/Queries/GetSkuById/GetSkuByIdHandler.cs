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
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.GetByIdAsync), "SKU not found", new { skuId = request.SkuId });

            return Errors.Inventory.SkuDoesNotExist;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully retrieved SKU by ID", new { skuId = request.SkuId, modelId = sku.ModelId.ToString() });

        return sku.ToResponse();

    }
}
