using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Application.Categories.Commands;
using YGZ.Catalog.Api.Contracts.CategoryRequest;

namespace YGZ.Catalog.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/categories")]
[OpenApiTag("categories", Description = "Manage categories.")]
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

    [HttpPost()]
    public async Task<IActionResult> CreateCategogy([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var cmd = _mapper.Map<CreateCategoryCommand>(request);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
