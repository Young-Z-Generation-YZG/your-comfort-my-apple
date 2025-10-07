using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api.Contracts.CategoryRequest;
using YGZ.Catalog.Application.Categories.Queries.GetCategories;
using YGZ.Catalog.Application.Categories.Commands.CreateCategory;

namespace YGZ.Catalog.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/categories")]
[OpenApiTag("Category Controllers", Description = "Manage categories.")]
//[ProtectedResource("categories")]
[AllowAnonymous]
public class CategoryController : ApiController
{
    private readonly ILogger<CategoryController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public CategoryController(ILogger<CategoryController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet()]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var query = new GetCategoriesQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPost()]
    public async Task<IActionResult> CreateCategogy([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<CreateCategoryCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
