namespace pruieba.Models;

/// <summary>
/// DTO para el endpoint de chat
/// </summary>
public class ChatRequest
{
    public string Message { get; set; } = string.Empty;
    public List<ChatMessage> History { get; set; } = new();
}

/// <summary>
/// DTO para el endpoint de análisis de imágenes
/// </summary>
public class AnalyzeImageRequest
{
    public string ImageBase64 { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
}

/// <summary>
/// DTO para el endpoint de generación de reportes
/// </summary>
public class ReportRequest
{
    public string Fecha { get; set; } = string.Empty;
    public int TotalVehiculos { get; set; }
    public List<MovimientoVehicular> Movimientos { get; set; } = new();
    public string TipoReporte { get; set; } = "resumen"; // "resumen", "anomalias", "ejecutivo"
}

public class MovimientoVehicular
{
    public string Placa { get; set; } = string.Empty;
    public string TipoVehiculo { get; set; } = string.Empty;
    public DateTime HoraEntrada { get; set; }
    public DateTime? HoraSalida { get; set; }
    public string EspacioAsignado { get; set; } = string.Empty;
    public int NumeroReasignaciones { get; set; }
    public int TiempoEmbarqueMinutos { get; set; }
}

/// <summary>
/// Respuesta genérica para todos los endpoints
/// </summary>
public class GeminiApiResponse
{
    public bool Success { get; set; }
    public string Response { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
    public int? TokensUsed { get; set; }
}
