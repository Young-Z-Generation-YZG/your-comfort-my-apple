using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.ProductItems.Extensions;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Entities;

namespace YGZ.Catalog.Application.ProductItems.Commands.CreateProductItem;

public class CreateProductItemCommandHandler : ICommandHandler<CreateProductItemCommand, bool>
{
    private readonly IMongoRepository<ProductItem> _productItemRepository;

    public CreateProductItemCommandHandler(IMongoRepository<ProductItem> productItemRepository)
    {
        _productItemRepository = productItemRepository;
    }

    public async Task<Result<bool>> Handle(CreateProductItemCommand request, CancellationToken cancellationToken)
    {
        ProductItem newProductItem = request.ToEntity();

        await _productItemRepository.InsertOneAsync(newProductItem);

        return true;
    }
}
