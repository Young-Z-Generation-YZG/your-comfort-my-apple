using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;

namespace YGZ.Catalog.Application.Categories.Commands.RemoveSubCategory;

public class RemoveSubCategoryHandler : ICommandHandler<RemoveSubCategoryCommand, bool>
{
    private readonly ILogger<RemoveSubCategoryHandler> _logger;
    private readonly IMongoRepository<Category, CategoryId> _mongoRepository;

    public RemoveSubCategoryHandler(ILogger<RemoveSubCategoryHandler> logger, IMongoRepository<Category, CategoryId> mongoRepository)
    {
        _logger = logger;
        _mongoRepository = mongoRepository;
    }

    public async Task<Result<bool>> Handle(RemoveSubCategoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
