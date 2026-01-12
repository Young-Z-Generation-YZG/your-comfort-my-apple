using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Requests.Commands.UpdateSkuRequest;

public sealed record UpdateSkuRequestCommand(string SkuRequestId, string State) : ICommand<bool>;
