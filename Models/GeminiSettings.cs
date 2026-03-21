namespace pruieba.Models;

/// <summary>
/// Configuración para el servicio de Google Gemini.
/// Se vincula automáticamente desde appsettings.json usando Options Pattern.
/// </summary>
public class GeminiSettings
{
    /// <summary>
    /// Clave de API de Google AI Studio (debe estar en variable de entorno GEMINI_API_KEY)
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del modelo a utilizar (ej: gemini-2.0-flash)
    /// </summary>
    public string ModelId { get; set; } = "gemini-2.0-flash";

    /// <summary>
    /// URL base de la API de Google Gemini
    /// </summary>
    public string BaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta/models";

    /// <summary>
    /// Máximo número de tokens en la respuesta generada
    /// </summary>
    public int MaxOutputTokens { get; set; } = 2048;

    /// <summary>
    /// Temperatura para la generación (0.0-2.0). Valores más altos = más creatividad
    /// </summary>
    public double Temperature { get; set; } = 0.7;

    /// <summary>
    /// Timeout en segundos para las peticiones HTTP
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
}
