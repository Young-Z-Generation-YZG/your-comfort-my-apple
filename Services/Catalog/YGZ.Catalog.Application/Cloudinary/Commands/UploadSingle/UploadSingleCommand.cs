

using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Cloudinary.Commands.UploadSingle;

public sealed record UploadSingleCommand(IFormFile File) : ICommand<ImageUploadResult> { }
