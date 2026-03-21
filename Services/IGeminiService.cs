using pruieba.Models;

namespace pruieba.Services;

/// <summary>
/// Interfaz para el servicio de integración con Google Gemini.
/// Define los métodos principales para interactuar con la IA.
/// </summary>
public interface IGeminiService
{
    /// <summary>
    /// Genera texto a partir de un prompt simple.
    /// </summary>
    /// <param name="prompt">Texto de entrada para el modelo</param>
    /// <returns>Respuesta generada por Gemini</returns>
    Task<string> GenerateTextAsync(string prompt);

    /// <summary>
    /// Mantiene una conversación con historial de mensajes.
    /// </summary>
    /// <param name="history">Historial de mensajes previos</param>
    /// <param name="newMessage">Nuevo mensaje del usuario</param>
    /// <returns>Respuesta del modelo en el contexto de la conversación</returns>
    Task<string> ChatAsync(List<ChatMessage> history, string newMessage);

    /// <summary>
    /// Analiza una imagen y responde preguntas sobre ella.
    /// </summary>
    /// <param name="base64Image">Imagen codificada en Base64</param>
    /// <param name="prompt">Pregunta o instrucción sobre la imagen</param>
    /// <returns>Análisis o descripción generada por el modelo</returns>
    Task<string> AnalyzeImageAsync(string base64Image, string prompt);

    /// <summary>
    /// Genera un reporte personalizado basado en datos estructurados.
    /// </summary>
    /// <param name="reportData">Datos del reporte en formato JSON</param>
    /// <param name="reportType">Tipo de reporte (resumen, anomalías, ejecutivo)</param>
    /// <returns>Reporte generado en lenguaje natural</returns>
    Task<string> GenerateReportAsync(string reportData, string reportType);
}
