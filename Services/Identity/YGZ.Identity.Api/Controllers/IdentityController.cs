using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YGZ.Identity.Api.Extensions.NewFolder;
using YGZ.Identity.Application.Identity.Commands.CreateUser;
using YGZ.Identity.Application.Samples.Commands;

namespace YGZ.Identity.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1)]
    public class IdentityController : ApiController
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public IdentityController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] CreateUserCommand request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(request, cancellationToken);

            return result.Match(Ok, HandleFailure);
        }
    }
}
