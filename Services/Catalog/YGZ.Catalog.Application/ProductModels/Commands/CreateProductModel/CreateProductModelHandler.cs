using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.ProductModels;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

public class CreateProductModelHandler : ICommandHandler<CreateProductModelCommand, bool>
{
    private readonly ILogger<CreateProductModelHandler> _logger;
    private readonly IMongoRepository<ProductModel, ModelId> _repository;

    public CreateProductModelHandler(ILogger<CreateProductModelHandler> logger, IMongoRepository<ProductModel, ModelId> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(CreateProductModelCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
