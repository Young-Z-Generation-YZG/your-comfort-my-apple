using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace YGZ.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1)]
    [ApiVersion(2, Deprecated = true)]
    public class SampleController : ControllerBase
    {
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
    }
}
