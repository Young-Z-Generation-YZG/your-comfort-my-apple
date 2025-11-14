using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;

namespace YGZ.Catalog.Application.Categories.Commands.AddSubCategory;

public class AddSubCategoryHandler : ICommandHandler<AddSubCategoryCommand, bool>
{
    private readonly ILogger<AddSubCategoryHandler> _logger;
    private readonly IMongoRepository<Category, CategoryId> _mongoRepository;

    public AddSubCategoryHandler(ILogger<AddSubCategoryHandler> logger, IMongoRepository<Category, CategoryId> mongoRepository)
    {
        _logger = logger;
        _mongoRepository = mongoRepository;
    }

    public async Task<Result<bool>> Handle(AddSubCategoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
