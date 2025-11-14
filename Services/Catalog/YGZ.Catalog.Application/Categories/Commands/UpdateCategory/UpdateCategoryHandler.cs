using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Errors;

namespace YGZ.Catalog.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryHandler : ICommandHandler<UpdateCategoryCommand, bool>
{
    private readonly ILogger<UpdateCategoryHandler> _logger;
    private readonly IMongoRepository<Category, CategoryId> _mongoRepository;

    public UpdateCategoryHandler(ILogger<UpdateCategoryHandler> logger, IMongoRepository<Category, CategoryId> mongoRepository)
    {
        _logger = logger;
        _mongoRepository = mongoRepository;
    }

    public async Task<Result<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var existingCategory = await _mongoRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (existingCategory is null)
        {
            return Errors.Category.DoesNotExist;
        }

        var parentCategory = await ResolveParentCategoryAsync(request, cancellationToken);
        if (parentCategory.IsFailure)
        {
            return parentCategory.Error;
        }

        var subCategories = await ResolveSubCategoriesAsync(request, cancellationToken);
        if (subCategories.IsFailure)
        {
            return subCategories.Error;
        }

        existingCategory.Update(request.Name,
                                request.Description,
                                request.Order,
                                parentCategory.Response,
                                subCategories.Response);

        var result = await _mongoRepository.UpdateAsync(request.CategoryId, existingCategory);

        return result;
    }

    private async Task<Result<Category?>> ResolveParentCategoryAsync(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ParentCategoryId))
        {
            return Result<Category?>.Success(null);
        }

        if (request.ParentCategoryId == request.CategoryId)
        {
            return Errors.Category.ParentDoesNotExist;
        }

        var parentCategory = await _mongoRepository.GetByIdAsync(request.ParentCategoryId, cancellationToken);

        if (parentCategory is null)
        {
            return Errors.Category.ParentDoesNotExist;
        }

        return parentCategory;
    }

    private async Task<Result<List<Category>?>> ResolveSubCategoriesAsync(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (request.SubCategoryIds is null)
        {
            return Result<List<Category>?>.Success(null);
        }

        var subCategories = new List<Category>();

        foreach (var subCategoryId in request.SubCategoryIds.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct())
        {
            if (subCategoryId == request.CategoryId)
            {
                continue;
            }

            var subCategory = await _mongoRepository.GetByIdAsync(subCategoryId, cancellationToken);

            if (subCategory is null)
            {
                continue;
            }

            subCategories.Add(subCategory);
        }

        return subCategories;
    }
}
