using Microsoft.Extensions.Logging;
using System.Linq;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.ProductModels;

namespace YGZ.Catalog.Application.Categories.Queries.GetCategoryDetails;

public class GetCategoryDetailsHandler : IQueryHandler<GetCategoryDetailsQuery, CategoryResponse>
{
    private readonly ILogger<GetCategoryDetailsHandler> _logger;
    private readonly IMongoRepository<Category, CategoryId> _mongoRepository;
    private readonly IMongoRepository<ProductModel, ModelId> _productModelRepository;

    public GetCategoryDetailsHandler(ILogger<GetCategoryDetailsHandler> logger,
                                     IMongoRepository<Category, CategoryId> mongoRepository,
                                     IMongoRepository<ProductModel, ModelId> productModelRepository)
    {
        _logger = logger;
        _mongoRepository = mongoRepository;
        _productModelRepository = productModelRepository;
    }

    public async Task<Result<CategoryResponse>> Handle(GetCategoryDetailsQuery request, CancellationToken cancellationToken)
    {
        var category = await _mongoRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            _logger.LogWarning("Category with id {CategoryId} was not found.", request.CategoryId);
            return Errors.Category.DoesNotExist;
        }

        var productModels = await _productModelRepository.GetAllAsync(cancellationToken);

        var categoryProductModels = productModels
            .Where(pm => pm.Category.Id.Value == category.Id.Value)
            .Select(pm => pm.ToResponse())
            .ToList();

        return category.ToResponse(categoryProductModels);
    }
}
