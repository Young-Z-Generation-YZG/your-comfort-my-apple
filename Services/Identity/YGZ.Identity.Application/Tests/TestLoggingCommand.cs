using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Tests;

public sealed record TestLoggingCommand : ICommand<bool>
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
}
