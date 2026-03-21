using Microsoft.AspNetCore.Mvc;
using pruieba.Models;
using pruieba.Services;
using System.Text.Json;

namespace pruieba.Controllers;

/// <summary>
/// Controlador REST para exponer funcionalidades de Google Gemini.
/// Proporciona endpoints para chat, análisis de imágenes y generación de reportes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GeminiController : ControllerBase
{
    private readonly IGeminiService _geminiService;
    private readonly ILogger<GeminiController> _logger;

    public GeminiController(IGeminiService geminiService, ILogger<GeminiController> logger)
    {
        _geminiService = geminiService;
        _logger = logger;
    }

    /// <summary>
    /// Endpoint para mantener conversaciones con el asistente IA.
    /// POST /api/gemini/chat
    /// </summary>
    /// <remarks>
    /// Ejemplo de uso para LogiFleet:
    /// {
    ///   "message": "¿Cómo registro la entrada de un camión de carga?",
    ///   "history": [
    ///     { "role": "model", "message": "Soy el asistente de LogiFleet. ¿En qué puedo ayudarte?" }
    ///   ]
    /// }
    /// </remarks>
    [HttpPost("chat")]
    [ProducesResponseType(typeof(GeminiApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GeminiApiResponse>> Chat([FromBody] ChatRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new GeminiApiResponse
                {
                    Success = false,
                    ErrorMessage = "El mensaje no puede estar vacío"
                });
            }

            _logger.LogInformation("Procesando chat - Mensaje: {Message}", request.Message);

            // Agregar contexto del sistema si es la primera interacción
            if (request.History == null || request.History.Count == 0)
            {
                request.History = new List<ChatMessage>
                {
                    new ChatMessage
                    {
                        Role = "user",
                        Message = @"Eres un asistente logístico del sistema LogiFleet para TRANSPORTES PREMIUM S.A.
                                   Ayuda a los operadores portuarios con preguntas sobre procedimientos de embarque,
                                   asignación de espacios y control de vehículos en ferries de la ruta Mazatlán-La Paz.
                                   Responde de forma breve, clara y profesional para personal no técnico."
                    },
                    new ChatMessage
                    {
                        Role = "model",
                        Message = "Entendido. Soy el asistente de LogiFleet y estoy aquí para ayudarte con las operaciones portuarias. ¿Qué necesitas?"
                    }
                };
            }

            var response = await _geminiService.ChatAsync(request.History, request.Message);

            return Ok(new GeminiApiResponse
            {
                Success = true,
                Response = response
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar chat");
            return StatusCode(500, new GeminiApiResponse
            {
                Success = false,
                ErrorMessage = "Error interno al procesar la conversación"
            });
        }
    }

    /// <summary>
    /// Endpoint para análisis de imágenes con visión artificial.
    /// POST /api/gemini/analyze
    /// </summary>
    /// <remarks>
    /// Ejemplo de uso para LogiFleet:
    /// {
    ///   "imageBase64": "iVBORw0KGgoAAAANSUhEUgAA...",
    ///   "prompt": "Analiza esta imagen de un documento vehicular. Extrae: número de placa, tipo de vehículo, nombre del propietario y fecha de vencimiento si aparece. Devuelve la información en formato JSON."
    /// }
    /// </remarks>
    [HttpPost("analyze")]
    [ProducesResponseType(typeof(GeminiApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GeminiApiResponse>> AnalyzeImage([FromBody] AnalyzeImageRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.ImageBase64))
            {
                return BadRequest(new GeminiApiResponse
                {
                    Success = false,
                    ErrorMessage = "La imagen en Base64 no puede estar vacía"
                });
            }

            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest(new GeminiApiResponse
                {
                    Success = false,
                    ErrorMessage = "El prompt no puede estar vacío"
                });
            }

            _logger.LogInformation("Procesando análisis de imagen");

            var response = await _geminiService.AnalyzeImageAsync(request.ImageBase64, request.Prompt);

            return Ok(new GeminiApiResponse
            {
                Success = true,
                Response = response
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al analizar imagen");
            return StatusCode(500, new GeminiApiResponse
            {
                Success = false,
                ErrorMessage = "Error interno al procesar la imagen"
            });
        }
    }

    /// <summary>
    /// Endpoint para generar reportes operativos en lenguaje natural.
    /// POST /api/gemini/report
    /// </summary>
    /// <remarks>
    /// Ejemplo de uso para LogiFleet:
    /// {
    ///   "fecha": "2024-01-15",
    ///   "totalVehiculos": 87,
    ///   "tipoReporte": "resumen",
    ///   "movimientos": [
    ///     {
    ///       "placa": "ABC-123",
    ///       "tipoVehiculo": "Automóvil",
    ///       "horaEntrada": "2024-01-15T08:30:00",
    ///       "horaSalida": "2024-01-15T09:15:00",
    ///       "espacioAsignado": "A-12",
    ///       "numeroReasignaciones": 0,
    ///       "tiempoEmbarqueMinutos": 45
    ///     }
    ///   ]
    /// }
    /// Tipos de reporte: "resumen", "anomalias", "ejecutivo"
    /// </remarks>
    [HttpPost("report")]
    [ProducesResponseType(typeof(GeminiApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GeminiApiResponse>> GenerateReport([FromBody] ReportRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Fecha))
            {
                return BadRequest(new GeminiApiResponse
                {
                    Success = false,
                    ErrorMessage = "La fecha del reporte es requerida"
                });
            }

            _logger.LogInformation("Generando reporte tipo: {TipoReporte} para fecha: {Fecha}",
                request.TipoReporte, request.Fecha);

            // Serializar los datos del reporte a JSON
            var reportDataJson = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var response = await _geminiService.GenerateReportAsync(reportDataJson, request.TipoReporte);

            return Ok(new GeminiApiResponse
            {
                Success = true,
                Response = response
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar reporte");
            return StatusCode(500, new GeminiApiResponse
            {
                Success = false,
                ErrorMessage = "Error interno al generar el reporte"
            });
        }
    }

    /// <summary>
    /// Endpoint de prueba básico para verificar conexión con Gemini.
    /// GET /api/gemini/test
    /// </summary>
    [HttpGet("test")]
    [ProducesResponseType(typeof(GeminiApiResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GeminiApiResponse>> Test()
    {
        try
        {
            _logger.LogInformation("Ejecutando prueba de conexión con Gemini");

            var testPrompt = "Responde con un mensaje breve confirmando que estás funcionando correctamente.";
            var response = await _geminiService.GenerateTextAsync(testPrompt);

            return Ok(new GeminiApiResponse
            {
                Success = true,
                Response = $"✅ Conexión exitosa con Gemini. Respuesta: {response}"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en prueba de conexión");
            return Ok(new GeminiApiResponse
            {
                Success = false,
                ErrorMessage = $"❌ Error de conexión: {ex.Message}"
            });
        }
    }
}
