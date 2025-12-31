using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Errors;

namespace YGZ.Catalog.Application.Categories.Commands.CreateCategory;

public class CreateCategoryHandler : ICommandHandler<CreateCategoryCommand, bool>
{
    private readonly ILogger<CreateCategoryHandler> _logger;
    private readonly IMongoRepository<Category, CategoryId> _repository;

    public CreateCategoryHandler(IMongoRepository<Category, CategoryId> repository, ILogger<CreateCategoryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<Result<bool>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category? parentCategory = null;

        if (!string.IsNullOrWhiteSpace(request.ParentId))
        {
            parentCategory = await _repository.GetByIdAsync(request.ParentId, cancellationToken);

            if (parentCategory is null)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_repository.GetByIdAsync), "Parent category not found", new { parentId = request.ParentId, name = request.Name });

                return Errors.Category.ParentDoesNotExist;
            }
        }

        Category newCategory = Category.Create(CategoryId.Create(),
                                               parentId: request.ParentId,
                                               name: request.Name,
                                               description: request.Description,
                                               order: request.Order,
                                               parentCategory: parentCategory);

        var result = await _repository.InsertOneAsync(newCategory);

        if (result.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.InsertOneAsync), "Failed to create category", new { name = request.Name, parentId = request.ParentId, error = result.Error });

            return result.Error;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully created category", new { categoryId = newCategory.Id.ToString(), name = request.Name, parentId = request.ParentId });

        return true;
    }
}
