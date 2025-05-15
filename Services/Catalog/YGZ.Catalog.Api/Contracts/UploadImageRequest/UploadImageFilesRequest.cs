namespace YGZ.Catalog.Api.Contracts.UploadImageRequest;

public sealed record UploadImageFilesRequest(List<IFormFile> Files) { }