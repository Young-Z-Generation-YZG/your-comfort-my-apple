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
    private readonly IMongoRepository<SKU, SkuId> _repository;
    private readonly ILogger<GetSkuByIdHandler> _logger;

    public GetSkuByIdHandler(IMongoRepository<SKU, SkuId> repository, ILogger<GetSkuByIdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<SkuResponse>> Handle(GetSkuByIdQuery request, CancellationToken cancellationToken)
    {
        var sku = await _repository.GetByIdAsync(request.SkuId, cancellationToken);

        if (sku is null)
        {
            return Errors.Inventory.SkuDoesNotExist;
        }

        return sku.ToResponse();

    }
}
