using Microsoft.Extensions.Options;
using pruieba.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace pruieba.Services;

/// <summary>
/// Servicio que gestiona la comunicación con la API de Google Gemini.
/// Implementa generación de texto, chat conversacional y análisis de imágenes.
/// </summary>
public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly GeminiSettings _settings;
    private readonly ILogger<GeminiService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public GeminiService(
        HttpClient httpClient,
        IOptions<GeminiSettings> settings,
        ILogger<GeminiService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;

        // Configurar opciones de serialización JSON
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        // Configurar HttpClient
        _httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>
    /// Genera texto simple a partir de un prompt
    /// </summary>
    public async Task<string> GenerateTextAsync(string prompt)
    {
        try
        {
            _logger.LogInformation("Generando texto con Gemini para prompt: {Prompt}", prompt.Substring(0, Math.Min(50, prompt.Length)));

            var request = new GeminiRequest
            {
                Contents = new List<Content>
                {
                    new Content
                    {
                        Role = "user",
                        Parts = new List<Part>
                        {
                            new Part { Text = prompt }
                        }
                    }
                },
                GenerationConfig = new GenerationConfig
                {
                    Temperature = _settings.Temperature,
                    MaxOutputTokens = _settings.MaxOutputTokens
                }
            };

            var response = await SendRequestAsync(request);
            return ExtractTextFromResponse(response);
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

            var request = new GeminiRequest
            {
                Contents = new List<Content>(),
                GenerationConfig = new GenerationConfig
                {
                    Temperature = _settings.Temperature,
                    MaxOutputTokens = _settings.MaxOutputTokens
                }
            };

            // Agregar historial al contexto
            foreach (var msg in history)
            {
                request.Contents.Add(new Content
                {
                    Role = msg.Role,
                    Parts = new List<Part> { new Part { Text = msg.Message } }
                });
            }

            // Agregar nuevo mensaje del usuario
            request.Contents.Add(new Content
            {
                Role = "user",
                Parts = new List<Part> { new Part { Text = newMessage } }
            });

            var response = await SendRequestAsync(request);
            return ExtractTextFromResponse(response);
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

            var request = new GeminiRequest
            {
                Contents = new List<Content>
                {
                    new Content
                    {
                        Role = "user",
                        Parts = new List<Part>
                        {
                            new Part
                            {
                                InlineData = new InlineData
                                {
                                    MimeType = "image/jpeg",
                                    Data = base64Image
                                }
                            },
                            new Part { Text = prompt }
                        }
                    }
                },
                GenerationConfig = new GenerationConfig
                {
                    Temperature = _settings.Temperature,
                    MaxOutputTokens = _settings.MaxOutputTokens
                }
            };

            var response = await SendRequestAsync(request);
            return ExtractTextFromResponse(response);
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

    /// <summary>
    /// Envía la petición HTTP a la API de Gemini
    /// </summary>
    private async Task<GeminiResponse> SendRequestAsync(GeminiRequest request)
    {
        try
        {
            // Construir URL del endpoint
            var endpoint = $"{_settings.BaseUrl}/{_settings.ModelId}:generateContent?key={_settings.ApiKey}";

            // Serializar el cuerpo de la petición
            var jsonContent = JsonSerializer.Serialize(request, _jsonOptions);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _logger.LogDebug("Enviando petición a Gemini: {Endpoint}", endpoint.Replace(_settings.ApiKey, "***"));

            // Enviar petición POST
            var httpResponse = await _httpClient.PostAsync(endpoint, httpContent);

            // Leer respuesta
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            // Validar código de estado HTTP
            if (!httpResponse.IsSuccessStatusCode)
            {
                _logger.LogError("Error HTTP {StatusCode}: {Content}", httpResponse.StatusCode, responseContent);
                throw new HttpRequestException($"Error de la API de Gemini: {httpResponse.StatusCode} - {responseContent}");
            }

            // Deserializar respuesta
            var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseContent, _jsonOptions);

            if (geminiResponse == null)
            {
                throw new InvalidOperationException("La respuesta de Gemini no pudo ser deserializada");
            }

            _logger.LogInformation("Respuesta recibida. Tokens usados: {Tokens}",
                geminiResponse.UsageMetadata?.TotalTokenCount ?? 0);

            return geminiResponse;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout al conectar con Gemini");
            throw new TimeoutException("La petición a Gemini excedió el tiempo límite", ex);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error HTTP al comunicarse con Gemini");
            throw;
        }
    }

    /// <summary>
    /// Extrae el texto generado de la respuesta de Gemini
    /// </summary>
    private string ExtractTextFromResponse(GeminiResponse response)
    {
        if (response.Candidates == null || response.Candidates.Count == 0)
        {
            _logger.LogWarning("La respuesta no contiene candidatos");
            return string.Empty;
        }

        var firstCandidate = response.Candidates[0];
        var parts = firstCandidate.Content?.Parts;

        if (parts == null || parts.Count == 0)
        {
            _logger.LogWarning("El candidato no contiene partes de texto");
            return string.Empty;
        }

        // Concatenar todos los textos de las partes
        var texts = parts.Where(p => !string.IsNullOrEmpty(p.Text)).Select(p => p.Text);
        return string.Join(" ", texts);
    }
}
