

using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Windows.Input;
using YGZ.Catalog.Application.Core.Abstractions.Messaging;

namespace YGZ.Catalog.Application.Uploading.Commands.UploadSingle;


public sealed record UploadSingleCommand(IFormFile File) : ICommand<ImageUploadResult> { }
