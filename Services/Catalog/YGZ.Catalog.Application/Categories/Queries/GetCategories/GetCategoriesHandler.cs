using Microsoft.Extensions.Logging;
using System.Linq;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.ProductModels;


namespace YGZ.Catalog.Application.Categories.Queries.GetCategories;

public class GetCategoriesHandler : IQueryHandler<GetCategoriesQuery, List<CategoryResponse>>
{
    private readonly ILogger<GetCategoriesHandler> _logger;
    private readonly IMongoRepository<Category, CategoryId> _repository;
    private readonly IMongoRepository<ProductModel, ModelId> _productModelRepository;

    public GetCategoriesHandler(ILogger<GetCategoriesHandler> logger,
                                IMongoRepository<Category, CategoryId> repository,
                                IMongoRepository<ProductModel, ModelId> productModelRepository)
    {
        _logger = logger;
        _repository = repository;
        _productModelRepository = productModelRepository;
    }

    public async Task<Result<List<CategoryResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categoriesTask = _repository.GetAllAsync(cancellationToken);
        var productModelsTask = _productModelRepository.GetAllAsync(cancellationToken);

        await Task.WhenAll(categoriesTask, productModelsTask);

        var categories = categoriesTask.Result;
        var productModels = productModelsTask.Result;

        var modelsLookup = productModels
            .Where(pm => !string.IsNullOrWhiteSpace(pm.Category.Id.Value))
            .GroupBy(pm => pm.Category.Id.Value!)
            .ToDictionary(group => group.Key, group => group.Select(pm => pm.ToResponse()).ToList());

        var response = categories.Select(category =>
        {
            modelsLookup.TryGetValue(category.Id.Value!, out var categoryModels);
            return category.ToResponse(categoryModels);
        }).ToList();

        return response;
    }
}
