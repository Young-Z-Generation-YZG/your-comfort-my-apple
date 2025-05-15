using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Catalog.Application.Cloudinary.Queries.GetImages;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api.Contracts.UploadImageRequest;
using YGZ.Catalog.Application.Cloudinary.Commands.UploadSingle;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.Catalog.Api.Controllers;

[Route("api/v{version:apiVersion}/upload")]
[OpenApiTag("Image Upload Controllers", Description = "Manage image file.")]
[AllowAnonymous]
public class UploadFileController : ApiController
{
    private readonly ILogger<UploadFileController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public UploadFileController(ILogger<UploadFileController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// [DONE] Get all images from cloudinary
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("images")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var cmd = new GetImagesQuery();

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(_mapper.Map<List<CloudinaryImageResponse>>(result.Resources)), onFailure: HandleFailure);
    }

    /// <summary>
    /// [DONE] Upload single image
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("single")]
    public async Task<IActionResult> UploadSingle([FromForm] UploadImageFileRequest request, CancellationToken cancellationToken = default)
    {
        var cmd = new UploadSingleCommand(request.File);

        var result = await _sender.Send(cmd, cancellationToken);

        return result.Match(onSuccess: result => Ok(_mapper.Map<CloudinaryImageResponse>(result)), onFailure: HandleFailure);
    }

    [HttpPost]
    [Route("multiple")]
    public async Task<IActionResult> UploadMultiple([FromForm] UploadImageFilesRequest request)
    {
        await Task.CompletedTask;

        return Ok();
    }

    [HttpPost]
    [Route("url")]
    public async Task<IActionResult> UploadByUrl([FromBody] UploadImageUrlRequest request)
    {
        await Task.CompletedTask;

        return Ok();
    }

    [HttpPost]
    [Route("urls")]
    public async Task<IActionResult> UploadByUrls([FromBody] UploadImageUrlsRequest request)
    {
        await Task.CompletedTask;

        return Ok();
    }

    [HttpDelete]
    [Route("delete/{publicId}")]
    public async Task<IActionResult> Delete(string publicId)
    {
        await Task.CompletedTask;

        return Ok();
    }
}
