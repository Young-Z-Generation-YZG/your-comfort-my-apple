using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using YGZ.Catalog.Api.Common.Extensions;
using YGZ.Catalog.Api.Common.SwaggerExamples;
using YGZ.Catalog.Application.Categories.Commands.CreateCategory;
using YGZ.Catalog.Contracts.Categories;

namespace YGZ.Catalog.Api.Controllers;

[Route("api/v{version:apiVersion}/categories")]
[ApiVersion(1)]
public class CategoryController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public CategoryController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerRequestExample(typeof(CreateCategoryRequest), typeof(CreateCategoryRequestExample))]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateCategoryCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
