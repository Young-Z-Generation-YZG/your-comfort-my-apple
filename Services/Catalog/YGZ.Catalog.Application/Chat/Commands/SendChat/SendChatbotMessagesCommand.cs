using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

namespace YGZ.Catalog.Application.Chat.Commands.SendChat;

public sealed record SendChatbotMessagesCommand : ICommand<ChatbotMessageResponse>
{
    public required List<ChatbotMessageCommand> ChatbotMessages { get; init; }
}


public sealed record ChatbotMessageCommand
{
    public required string Role { get; init; }
    public required string Content { get; init; }
}