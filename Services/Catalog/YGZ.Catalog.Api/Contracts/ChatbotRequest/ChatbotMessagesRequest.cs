using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.ChatRequest;

public sealed record ChatbotMessagesRequest
{
    [Required]
    [JsonPropertyName("chatbot_messages")]
    public required List<ChatbotMessageRequest> ChatbotMessages { get; init; }
}

public sealed record ChatbotMessageRequest
{
    [Required]
    [JsonPropertyName("role")]
    public required string Role { get; init; }

    [Required]
    [JsonPropertyName("content")]
    public required string Content { get; init; }
}
