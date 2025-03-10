using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace YGZ.Basket.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/baskets")]
[OpenApiTag("baskets", Description = "Manage baskets.")]
//[ProtectedResource("baskets")]
[AllowAnonymous]
public class BasketController : ApiController
{
    private readonly ILogger<BasketController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public BasketController(ILogger<BasketController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    //[HttpPost()]
    //public async Task<IActionResult> CreateCategogy([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    //{
    //    var cmd = _mapper.Map<CreateCategoryCommand>(request);

    //    var result = await _sender.Send(cmd, cancellationToken);

    //    return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    //}
}
