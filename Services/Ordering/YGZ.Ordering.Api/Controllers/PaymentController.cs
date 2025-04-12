using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Ordering.Api.Contracts;
using YGZ.Ordering.Application.Payments.Commands;
using YGZ.BuildingBlocks.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace YGZ.Ordering.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/orders/payment")]
[OpenApiTag("payment", Description = "Payment check.")]
[AllowAnonymous]
public class PaymentController : ApiController
{
    private readonly ILogger<PaymentController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PaymentController(ILogger<PaymentController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPatch("vnpay-ipn-callback")]
    public async Task<IActionResult> VnpayIpn([FromBody] IpnCheckRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<IpnCheckCommand>(request);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
