using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using YGZ.Identity.Api.Extensions.NewFolder;
using YGZ.Identity.Application.Samples.Commands;
using YGZ.Identity.Contracts.Samples;

namespace YGZ.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1)]
    [ApiVersion(2, Deprecated = true)]
    public class SampleController : ApiController
    {
        private readonly ISender _sender;
        public readonly IMapper _mapper;

        public SampleController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the version 1 of the API
        /// </summary>
        /// <returns>Sample API</returns>
        [HttpGet]
        //[Route("")]
        [MapToApiVersion(1)]
        public IActionResult Get1()
        {
            return Ok("v1");
            //throw new Exception("test");
        }

        [HttpGet]
        //[Route("")]
        [MapToApiVersion(2)]
        public IActionResult Get2()
        {
            return Ok("v2");
        }

        [HttpPost]
        [MapToApiVersion(1)]
        public async Task<IActionResult> SamplePost(CreateSampleRequest request)
        {
            var cmd = _mapper.Map<CreateSampleCommand>(request);

            var result = await _sender.Send(cmd);

            return result.Match(Ok, HandleFailure);
        }
    }
}
