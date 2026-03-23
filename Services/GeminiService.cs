using Microsoft.Extensions.Options;
using pruieba.Models;
using Google.GenAI;
using GeminiTypes = Google.GenAI.Types;

namespace pruieba.Services;

/// <summary>
/// Servicio que gestiona la comunicación con la API de Google Gemini usando el SDK oficial Google.GenAI.
/// Implementa generación de texto, chat conversacional y análisis de imágenes.
/// </summary>
public class GeminiService : IGeminiService
{
    private readonly GeminiSettings _settings;
    private readonly ILogger<GeminiService> _logger;
    private readonly Client _client;

    public GeminiService(
        IOptions<GeminiSettings> settings,
        ILogger<GeminiService> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        // Inicializar el cliente de Gemini con la API Key
        _client = new Client(apiKey: _settings.ApiKey);
    }

    /// <summary>
    /// Genera texto simple a partir de un prompt
    /// </summary>
    public async Task<string> GenerateTextAsync(string prompt)
    {
        try
        {
            _logger.LogInformation("Generando texto con Gemini para prompt: {Prompt}", 
                prompt.Substring(0, Math.Min(50, prompt.Length)));

            // Generar contenido usando el SDK oficial
            var response = await _client.Models.GenerateContentAsync(
                model: _settings.ModelId,
                contents: prompt
            );

            // Extraer texto de la respuesta
            var text = response?.Candidates?[0]?.Content?.Parts?[0]?.Text;

            if (string.IsNullOrEmpty(text))
            {
                _logger.LogWarning("La respuesta de Gemini está vacía");
                return string.Empty;
            }

            _logger.LogInformation("Respuesta generada exitosamente");

            return text;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar texto con Gemini");
            throw new ApplicationException("Error al comunicarse con el servicio de IA", ex);
        }
    }

    /// <summary>
    /// Mantiene una conversación con historial
    /// </summary>
    public async Task<string> ChatAsync(List<ChatMessage> history, string newMessage)
    {
        try
        {
            _logger.LogInformation("Iniciando chat con historial de {Count} mensajes", history.Count);

            // Construir el contenido con historial
            var contents = new List<GeminiTypes.Content>();

            // Agregar historial previo
            foreach (var msg in history)
            {
                contents.Add(new GeminiTypes.Content
                {
                    Role = msg.Role,
                    Parts = new List<GeminiTypes.Part> { new GeminiTypes.Part { Text = msg.Message } }
                });
            }

            // Agregar nuevo mensaje del usuario
            contents.Add(new GeminiTypes.Content
            {
                Role = "user",
                Parts = new List<GeminiTypes.Part> { new GeminiTypes.Part { Text = newMessage } }
            });

            // Generar respuesta
            var response = await _client.Models.GenerateContentAsync(
                model: _settings.ModelId,
                contents: contents
            );

            var text = response?.Candidates?[0]?.Content?.Parts?[0]?.Text;

            if (string.IsNullOrEmpty(text))
            {
                _logger.LogWarning("La respuesta del chat está vacía");
                return string.Empty;
            }

            _logger.LogInformation("Chat completado");

            return text;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en conversación con Gemini");
            throw new ApplicationException("Error al procesar el chat", ex);
        }
    }

    /// <summary>
    /// Analiza una imagen codificada en Base64
    /// </summary>
    public async Task<string> AnalyzeImageAsync(string base64Image, string prompt)
    {
        try
        {
            _logger.LogInformation("Analizando imagen con Gemini");

            // Limpiar el prefijo data:image si existe
            if (base64Image.Contains(","))
            {
                base64Image = base64Image.Split(',')[1];
            }

            // Convertir Base64 a bytes
            var imageBytes = Convert.FromBase64String(base64Image);

            // Crear contenido multimodal (texto + imagen) usando FileData
            var content = new GeminiTypes.Content
            {
                Role = "user",
                Parts = new List<GeminiTypes.Part>
                {
                    new GeminiTypes.Part { Text = prompt },
                    new GeminiTypes.Part
                    {
                        // Usar FileData para imágenes inline
                        FileData = new GeminiTypes.FileData
                        {
                            MimeType = "image/jpeg",
                            FileUri = $"data:image/jpeg;base64,{base64Image}"
                        }
                    }
                }
            };

            // Generar respuesta con análisis de imagen
            var response = await _client.Models.GenerateContentAsync(
                model: _settings.ModelId,
                contents: new List<GeminiTypes.Content> { content }
            );

            var text = response?.Candidates?[0]?.Content?.Parts?[0]?.Text;

            if (string.IsNullOrEmpty(text))
            {
                _logger.LogWarning("La respuesta del análisis de imagen está vacía");
                return string.Empty;
            }

            _logger.LogInformation("Imagen analizada exitosamente");

            return text;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al analizar imagen con Gemini");
            throw new ApplicationException("Error al procesar la imagen", ex);
        }
    }

    /// <summary>
    /// Genera reportes personalizados basados en datos estructurados
    /// </summary>
    public async Task<string> GenerateReportAsync(string reportData, string reportType)
    {
        try
        {
            _logger.LogInformation("Generando reporte tipo: {ReportType}", reportType);

            // Construir el prompt según el tipo de reporte
            string systemPrompt = reportType.ToLower() switch
            {
                "resumen" => @"Analiza los siguientes datos de operación del ferry y genera un resumen ejecutivo 
                              en español de máximo 5 oraciones destacando: total de vehículos, incidencias, 
                              tiempo promedio de embarque y recomendaciones.",

                "anomalias" => @"Revisa este historial de movimientos e identifica patrones inusuales como: 
                               vehículos que tardaron más de 30 minutos en embarcar, espacios que se reasignaron 
                               más de 2 veces, o entradas fuera del horario establecido (06:00-22:00). 
                               Lista las anomalías brevemente.",

                "ejecutivo" => @"Genera un reporte ejecutivo profesional para la gerencia de TRANSPORTES PREMIUM S.A. 
                               Incluye métricas clave, análisis de eficiencia operativa y recomendaciones estratégicas. 
                               Formato: introducción, estadísticas principales, hallazgos y conclusiones.",

                _ => "Analiza los siguientes datos y genera un resumen en español."
            };

            string fullPrompt = $"{systemPrompt}\n\nDatos:\n{reportData}";

            return await GenerateTextAsync(fullPrompt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar reporte tipo: {ReportType}", reportType);
            throw new ApplicationException($"Error al generar reporte de tipo {reportType}", ex);
        }
    }
}
