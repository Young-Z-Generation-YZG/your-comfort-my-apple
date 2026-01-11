using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.Catalog.Application.Chat.Commands.SendChat;

public class SendChatbotMessagesHandler : ICommandHandler<SendChatbotMessagesCommand, ChatbotMessageResponse>
{
    private readonly ILogger<SendChatbotMessagesHandler> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    private const string SystemPrompt = @"Bạn là một trợ lý bán hàng thân thiện và chuyên nghiệp của YB Store - cửa hàng chuyên bán các sản phẩm Apple chính hãng bao gồm iPhone, iPad, MacBook, Apple Watch, AirPods và các phụ kiện. 

Nhiệm vụ của bạn:
- Tư vấn sản phẩm phù hợp với nhu cầu khách hàng
- Giải đáp thắc mắc về thông số kỹ thuật, giá cả, bảo hành
- Hỗ trợ so sánh các sản phẩm
- Cung cấp thông tin về chương trình khuyến mãi nếu có
- Hướng dẫn quy trình mua hàng và thanh toán

Phong cách giao tiếp:
- Thân thiện, lịch sự nhưng chuyên nghiệp
- Trả lời ngắn gọn, súc tích nhưng đầy đủ thông tin
- Sử dụng emoji phù hợp để tạo cảm giác thân thiện
- Luôn sẵn sàng hỗ trợ thêm nếu khách hàng cần";

    public SendChatbotMessagesHandler(
        ILogger<SendChatbotMessagesHandler> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<Result<ChatbotMessageResponse>> Handle(SendChatbotMessagesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(":::[CommandHandler:{CommandHandler}]::: Information message: {Message}, Parameters: {@Parameters}",
            nameof(SendChatbotMessagesHandler), "Processing chat request", new { messageCount = request.ChatbotMessages.Count });

        var apiKey = _configuration["OpenRouterSettings:ApiKey"];
        var model = _configuration["OpenRouterSettings:Model"] ?? "openai/gpt-4o-mini";

        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogError(":::[CommandHandler:{CommandHandler}][Result:Error]::: Error message: {Message}",
                nameof(SendChatbotMessagesHandler), "OpenRouter API key is not configured");

            return Error.BadRequest("Chat.MissingApiKey", "OpenRouter API key is not configured", "CatalogService");
        }

        try
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            client.DefaultRequestHeaders.Add("HTTP-Referer", "https://ybstore.com");
            client.DefaultRequestHeaders.Add("X-Title", "YB Store");

            var messages = new List<object>
            {
                new { role = "system", content = SystemPrompt }
            };

            foreach (var msg in request.ChatbotMessages)
            {
                messages.Add(new { role = msg.Role.ToLower(), content = msg.Content });
            }

            var requestBody = new
            {
                model = model,
                messages = messages,
                max_tokens = 1000,
                temperature = 0.7
            };

            _logger.LogInformation(":::[CommandHandler:{CommandHandler}]::: Information message: {Message}, Parameters: {@Parameters}",
                nameof(SendChatbotMessagesHandler), "Sending request to OpenRouter", new { model });

            var response = await client.PostAsJsonAsync(
                "https://openrouter.ai/api/v1/chat/completions",
                requestBody,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError(":::[CommandHandler:{CommandHandler}][Result:Error]::: Error message: {Message}, Parameters: {@Parameters}",
                    nameof(SendChatbotMessagesHandler), "OpenRouter API request failed", new { statusCode = response.StatusCode, error = errorContent });

                return Error.BadRequest("Chat.ApiError", $"OpenRouter API error: {response.StatusCode}", "CatalogService");
            }

            var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
            var assistantMessage = jsonResponse
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "";

            _logger.LogInformation(":::[CommandHandler:{CommandHandler}]::: Information message: {Message}",
                nameof(SendChatbotMessagesHandler), "Chat response generated successfully");

            return new ChatbotMessageResponse
            {
                Content = assistantMessage,
                Role = "assistant"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ":::[CommandHandler:{CommandHandler}][Exception:{ExceptionType}]::: Error message: {Message}",
                nameof(SendChatbotMessagesHandler), ex.GetType().Name, ex.Message);

            return Error.BadRequest("Chat.Exception", $"Failed to process chat: {ex.Message}", "CatalogService");
        }
    }
}
