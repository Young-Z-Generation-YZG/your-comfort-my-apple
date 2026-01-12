using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.Catalog.Application.Abstractions.Caching;

namespace YGZ.Catalog.Application.Chat.Commands.SendChat;

public class SendChatbotMessagesHandler : ICommandHandler<SendChatbotMessagesCommand, ChatbotMessageResponse>
{
    private readonly ILogger<SendChatbotMessagesHandler> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IProductCatalogCacheService _productCatalogCacheService;

    private const string BaseSystemPrompt = @"Bạn là một trợ lý bán hàng thân thiện và chuyên nghiệp của YB Store - cửa hàng chuyên bán các sản phẩm Apple chính hãng.

🎯 NGUYÊN TẮC QUAN TRỌNG:
- Bạn CHỈ được tư vấn về các sản phẩm có trong danh mục dưới đây
- Nếu khách hỏi về sản phẩm KHÔNG CÓ trong danh mục, hãy trả lời: 'Xin lỗi, hiện tại YB Store chưa có sản phẩm này. Bạn có muốn tôi tư vấn các sản phẩm tương tự mà chúng tôi đang có không?'
- Khi tư vấn, hãy sử dụng thông tin chính xác từ danh mục sản phẩm

📋 Nhiệm vụ của bạn:
- Tư vấn sản phẩm phù hợp với nhu cầu khách hàng
- Giải đáp thắc mắc về thông số kỹ thuật, giá cả, bảo hành
- Hỗ trợ so sánh các sản phẩm
- Cung cấp thông tin về chương trình khuyến mãi nếu có
- Hướng dẫn quy trình mua hàng và thanh toán

💬 Phong cách giao tiếp:
- Thân thiện, lịch sự nhưng chuyên nghiệp
- Trả lời ngắn gọn, súc tích nhưng đầy đủ thông tin
- Sử dụng emoji phù hợp để tạo cảm giác thân thiện
- Luôn sẵn sàng hỗ trợ thêm nếu khách hàng cần";

    public SendChatbotMessagesHandler(
        ILogger<SendChatbotMessagesHandler> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        IProductCatalogCacheService productCatalogCacheService)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _productCatalogCacheService = productCatalogCacheService;
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
            // Get product catalog from cache for context
            var productCatalog = await _productCatalogCacheService.GetProductCatalogSummaryAsync(cancellationToken);
            var systemPrompt = BuildSystemPrompt(productCatalog);

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            client.DefaultRequestHeaders.Add("HTTP-Referer", "https://ybstore.com");
            client.DefaultRequestHeaders.Add("X-Title", "YB Store");

            var messages = new List<object>
            {
                new { role = "system", content = systemPrompt }
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
                nameof(SendChatbotMessagesHandler), "Sending request to OpenRouter with product context", new { model, hasProductContext = !string.IsNullOrEmpty(productCatalog) });

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

    private string BuildSystemPrompt(string? productCatalog)
    {
        if (string.IsNullOrEmpty(productCatalog))
        {
            return BaseSystemPrompt + "\n\n⚠️ Lưu ý: Không thể tải danh mục sản phẩm. Vui lòng thông báo khách hàng liên hệ trực tiếp cửa hàng.";
        }

        return $@"{BaseSystemPrompt}

📦 DANH MỤC SẢN PHẨM HIỆN CÓ TẠI YB STORE:
{productCatalog}

Hãy sử dụng thông tin trên để tư vấn chính xác cho khách hàng.";
    }
}
