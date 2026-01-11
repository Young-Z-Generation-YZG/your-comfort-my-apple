using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api.Contracts.ChatRequest;
using YGZ.Catalog.Application.Chat.Commands.SendChat;

namespace YGZ.Catalog.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/chatbot/messages")]
[OpenApiTag("Chat bot Controllers", Description = "AI Chatbot for customer support.")]
[AllowAnonymous]
public class ChatbotController : ApiController
{
    private readonly ILogger<ChatbotController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ChatbotController(ILogger<ChatbotController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> SendChat([FromBody] ChatbotMessagesRequest request, CancellationToken cancellationToken)
    {
        var messages = request.ChatbotMessages.Select(m => new ChatbotMessageCommand
        {
            Role = m.Role,
            Content = m.Content
        }).ToList();

        var command = new SendChatbotMessagesCommand { ChatbotMessages = messages };

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(onSuccess: response => Ok(response), onFailure: HandleFailure);
    }
}
