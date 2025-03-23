
using MapsterMapper;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Core.Abstractions.Data;

namespace YGZ.Catalog.Application.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, List<CategoryResponse>>
{
    private readonly IMongoRepository<Category> _repository;
    private readonly IMapper _mapper;

    public GetCategoriesQueryHandler(IMongoRepository<Category> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<CategoryResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repository.GetAllAsync();

        List<CategoryResponse> response = MapToResponse(categories);

        return response;
    }

    private List<CategoryResponse> MapToResponse(List<Category> categories)
    {
        List<CategoryResponse> res = categories.Select(c =>
        {
            var category = _mapper.Map<CategoryResponse>(c);

            return category;
        }).ToList();

        return res;
    }
}
