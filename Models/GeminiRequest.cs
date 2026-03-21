using System.Text.Json.Serialization;

namespace pruieba.Models;

/// <summary>
/// Estructura de la petición para la API de Google Gemini
/// </summary>
public class GeminiRequest
{
    [JsonPropertyName("contents")]
    public List<Content> Contents { get; set; } = new();

    [JsonPropertyName("generationConfig")]
    public GenerationConfig? GenerationConfig { get; set; }

    [JsonPropertyName("safetySettings")]
    public List<SafetySetting>? SafetySettings { get; set; }
}

public class Content
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = "user"; // "user" o "model"

    [JsonPropertyName("parts")]
    public List<Part> Parts { get; set; } = new();
}

public class Part
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("inline_data")]
    public InlineData? InlineData { get; set; }
}

public class InlineData
{
    [JsonPropertyName("mime_type")]
    public string MimeType { get; set; } = "image/jpeg";

    [JsonPropertyName("data")]
    public string Data { get; set; } = string.Empty; // Base64 encoded
}

public class GenerationConfig
{
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; } = 0.7;

    [JsonPropertyName("maxOutputTokens")]
    public int MaxOutputTokens { get; set; } = 2048;

    [JsonPropertyName("topP")]
    public double? TopP { get; set; }

    [JsonPropertyName("topK")]
    public int? TopK { get; set; }
}

public class SafetySetting
{
    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("threshold")]
    public string Threshold { get; set; } = "BLOCK_MEDIUM_AND_ABOVE";
}
