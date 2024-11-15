

using Microsoft.AspNetCore.Http;

namespace YGZ.Catalog.Contracts.Common;

public sealed record UploadImageFileRequest(IFormFile File) { }

public sealed record UploadImageFilesRequest(List<IFormFile> Files) { }

public sealed record UploadImageUrlRequest(string Url) { }

public sealed record UploadImageUrlsRequest(string Urls) { }