
using YGZ.Catalog.Application.Core.Abstractions.Messaging;

namespace YGZ.Catalog.Application.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(string Name, string Description, string ParentId) : ICommand<bool> { }
