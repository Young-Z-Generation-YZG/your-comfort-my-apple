using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using MediatR;

namespace YGZ.Ordering.Api.Controllers;

[Route("api/v{version:apiVersion}/orders")]
[ApiVersion(1)]
public class OrderController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public OrderController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
}
