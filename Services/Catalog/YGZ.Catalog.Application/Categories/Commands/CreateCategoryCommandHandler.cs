

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Categories.Extensions;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;

namespace YGZ.Catalog.Application.Categories.Commands;

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, bool>
{
    private readonly IMongoRepository<Category> _repository;

    public CreateCategoryCommandHandler(IMongoRepository<Category> repository)
    {
        _repository = repository;
    }
    public async Task<Result<bool>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (request.ParentId is not null)
        {
            var parentCategory = await _repository.GetByIdAsync(request.ParentId);

            if (parentCategory is null)
            {
                return Errors.Category.DoesNotExist;
            }
        }

            await _repository.InsertOneAsync(request.ToEntity());

        return true;
    }
}
