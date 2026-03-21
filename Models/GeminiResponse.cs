using System.Text.Json.Serialization;

namespace pruieba.Models;

/// <summary>
/// Estructura de la respuesta de la API de Google Gemini
/// </summary>
public class GeminiResponse
{
    [JsonPropertyName("candidates")]
    public List<Candidate> Candidates { get; set; } = new();

    [JsonPropertyName("promptFeedback")]
    public PromptFeedback? PromptFeedback { get; set; }

    [JsonPropertyName("usageMetadata")]
    public UsageMetadata? UsageMetadata { get; set; }
}

public class Candidate
{
    [JsonPropertyName("content")]
    public Content Content { get; set; } = new();

    [JsonPropertyName("finishReason")]
    public string? FinishReason { get; set; }

    [JsonPropertyName("safetyRatings")]
    public List<SafetyRating>? SafetyRatings { get; set; }

    [JsonPropertyName("index")]
    public int Index { get; set; }
}

public class SafetyRating
{
    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("probability")]
    public string Probability { get; set; } = string.Empty;
}

public class PromptFeedback
{
    [JsonPropertyName("safetyRatings")]
    public List<SafetyRating>? SafetyRatings { get; set; }
}

public class UsageMetadata
{
    [JsonPropertyName("promptTokenCount")]
    public int PromptTokenCount { get; set; }

    [JsonPropertyName("candidatesTokenCount")]
    public int CandidatesTokenCount { get; set; }

    [JsonPropertyName("totalTokenCount")]
    public int TotalTokenCount { get; set; }
}

/// <summary>
/// Mensaje individual en el historial de chat
/// </summary>
public class ChatMessage
{
    public string Role { get; set; } = "user"; // "user" o "model"
    public string Message { get; set; } = string.Empty;
}
