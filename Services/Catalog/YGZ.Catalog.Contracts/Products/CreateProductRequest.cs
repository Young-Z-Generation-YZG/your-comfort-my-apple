using Microsoft.AspNetCore.Http;

namespace YGZ.Catalog.Contracts.Products;

public sealed record CreateProductRequest(string Name, IFormFile[] Files) { }
