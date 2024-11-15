using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using YGZ.Catalog.Api.Common.Extensions;
using YGZ.Catalog.Application.Uploading.Commands.UploadSingle;
using YGZ.Catalog.Application.Uploading.Queries.GetImages;
using YGZ.Catalog.Contracts.Common;
using YGZ.Catalog.Contracts.Products;


namespace YGZ.Catalog.Api.Controllers;

[Route("api/v{version:apiVersion}/upload")]
[ApiVersion(1)]
public class UploadController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public UploadController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("images")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var cmd = new GetImagesQuery();

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(_mapper.Map<List<UploadImageResponse>>(result.Resources)), onFailure: HandleFailure);
    }

    [HttpPost]
    [Route("single")]
    public async Task<IActionResult> UploadSingle([FromForm] UploadImageFileRequest request, CancellationToken cancellationToken = default)
    {
        var cmd = new UploadSingleCommand(request.File);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(_mapper.Map<UploadImageResponse>(result)), onFailure: HandleFailure);
    }

    [HttpPost]
    [Route("multiple")]
    public async Task<IActionResult> UploadMultiple([FromForm] UploadImageFilesRequest request)
    {
        Console.WriteLine("UploadMultiple" + request.Files);
        await Task.CompletedTask;

        return Ok();
    }

    [HttpPost]
    [Route("url")]
    public async Task<IActionResult> UploadByUrl([FromBody] UploadImageUrlRequest request)
    {
        Console.WriteLine("UploadByUrl" + request.Url);
        await Task.CompletedTask;

        return Ok();
    }

    [HttpPost]
    [Route("urls")]
    public async Task<IActionResult> UploadByUrls([FromBody] UploadImageUrlsRequest request)
    {
        Console.WriteLine("UploadByUrls" + request.Urls);
        await Task.CompletedTask;

        return Ok();
    }

}
