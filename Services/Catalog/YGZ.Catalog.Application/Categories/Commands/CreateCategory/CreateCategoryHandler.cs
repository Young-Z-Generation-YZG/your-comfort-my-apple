using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Errors;

namespace YGZ.Catalog.Application.Categories.Commands.CreateCategory;

public class CreateCategoryHandler : ICommandHandler<CreateCategoryCommand, bool>
{
    private readonly IMongoRepository<Category, CategoryId> _repository;

    public CreateCategoryHandler(IMongoRepository<Category, CategoryId> repository)
    {
        _repository = repository;
    }
    public async Task<Result<bool>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category? parentCategory = null;

        if (!string.IsNullOrWhiteSpace(request.ParentId))
        {
            parentCategory = await _repository.GetByIdAsync(request.ParentId, cancellationToken);

            if (parentCategory is null)
            {
                return Errors.Category.ParentDoesNotExist;
            }
        }

        Category newCategory = Category.Create(CategoryId.Create(),
                                               name: request.Name,
                                               description: request.Description,
                                               request.Order,
                                               parentCategory);

        await _repository.InsertOneAsync(newCategory);

        return true;
    }
}
