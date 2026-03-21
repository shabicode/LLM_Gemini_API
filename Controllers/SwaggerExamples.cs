using Microsoft.AspNetCore.Mvc;
using pruieba.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace pruieba.Controllers;

/// <summary>
/// Ejemplos de peticiones para usar en Swagger
/// </summary>
public static class SwaggerExamples
{
    /// <summary>
    /// Ejemplo de request para el endpoint de Chat
    /// </summary>
    public static ChatRequest ChatExample => new()
    {
        Message = "¿Cómo registro la entrada de un camión de carga de 12 metros?",
        History = new List<ChatMessage>
        {
            new() { Role = "user", Message = "Hola, necesito ayuda con las operaciones del ferry" },
            new() { Role = "model", Message = "¡Por supuesto! Soy el asistente de LogiFleet. ¿Qué necesitas saber?" }
        }
    };

    /// <summary>
    /// Ejemplo de request para análisis de imágenes
    /// </summary>
    public static AnalyzeImageRequest AnalyzeImageExample => new()
    {
        ImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==",
        Prompt = "Analiza esta imagen de una tarjeta de circulación. Extrae: placa, tipo de vehículo, propietario y vigencia. Devuelve en formato JSON."
    };

    /// <summary>
    /// Ejemplo de request para reportes - Resumen
    /// </summary>
    public static ReportRequest ReportResumenExample => new()
    {
        Fecha = DateTime.Today.ToString("yyyy-MM-dd"),
        TotalVehiculos = 87,
        TipoReporte = "resumen",
        Movimientos = new List<MovimientoVehicular>
        {
            new()
            {
                Placa = "ABC-123",
                TipoVehiculo = "Automóvil",
                HoraEntrada = DateTime.Parse("2024-01-15T08:30:00"),
                HoraSalida = DateTime.Parse("2024-01-15T09:15:00"),
                EspacioAsignado = "A-12",
                NumeroReasignaciones = 0,
                TiempoEmbarqueMinutos = 45
            },
            new()
            {
                Placa = "XYZ-789",
                TipoVehiculo = "Camioneta",
                HoraEntrada = DateTime.Parse("2024-01-15T09:00:00"),
                HoraSalida = DateTime.Parse("2024-01-15T09:30:00"),
                EspacioAsignado = "B-08",
                NumeroReasignaciones = 1,
                TiempoEmbarqueMinutos = 30
            },
            new()
            {
                Placa = "DEF-456",
                TipoVehiculo = "Camión",
                HoraEntrada = DateTime.Parse("2024-01-15T10:15:00"),
                HoraSalida = DateTime.Parse("2024-01-15T11:00:00"),
                EspacioAsignado = "C-05",
                NumeroReasignaciones = 0,
                TiempoEmbarqueMinutos = 45
            }
        }
    };

    /// <summary>
    /// Ejemplo de request para reportes - Detección de Anomalías
    /// </summary>
    public static ReportRequest ReportAnomaliasExample => new()
    {
        Fecha = DateTime.Today.ToString("yyyy-MM-dd"),
        TotalVehiculos = 45,
        TipoReporte = "anomalias",
        Movimientos = new List<MovimientoVehicular>
        {
            new()
            {
                Placa = "AAA-111",
                TipoVehiculo = "Automóvil",
                HoraEntrada = DateTime.Parse("2024-01-15T08:30:00"),
                HoraSalida = DateTime.Parse("2024-01-15T09:15:00"),
                EspacioAsignado = "A-12",
                NumeroReasignaciones = 0,
                TiempoEmbarqueMinutos = 20
            },
            new()
            {
                Placa = "BBB-222",
                TipoVehiculo = "Camión",
                HoraEntrada = DateTime.Parse("2024-01-15T23:45:00"), // ❌ Fuera de horario
                HoraSalida = DateTime.Parse("2024-01-16T00:30:00"),
                EspacioAsignado = "C-08",
                NumeroReasignaciones = 3, // ❌ Muchas reasignaciones
                TiempoEmbarqueMinutos = 65 // ❌ Tiempo excesivo
            },
            new()
            {
                Placa = "CCC-333",
                TipoVehiculo = "Autobús",
                HoraEntrada = DateTime.Parse("2024-01-15T14:00:00"),
                HoraSalida = DateTime.Parse("2024-01-15T15:15:00"),
                EspacioAsignado = "C-02",
                NumeroReasignaciones = 4, // ❌ Reasignaciones excesivas
                TiempoEmbarqueMinutos = 75 // ❌ Tiempo muy largo
            }
        }
    };

    /// <summary>
    /// Ejemplo de request para reportes - Reporte Ejecutivo
    /// </summary>
    public static ReportRequest ReportEjecutivoExample => new()
    {
        Fecha = DateTime.Today.ToString("yyyy-MM-dd"),
        TotalVehiculos = 156,
        TipoReporte = "ejecutivo",
        Movimientos = new List<MovimientoVehicular>
        {
            new()
            {
                Placa = "MAZ-001",
                TipoVehiculo = "Automóvil",
                HoraEntrada = DateTime.Parse("2024-01-15T06:15:00"),
                HoraSalida = DateTime.Parse("2024-01-15T06:45:00"),
                EspacioAsignado = "A-01",
                NumeroReasignaciones = 0,
                TiempoEmbarqueMinutos = 30
            },
            new()
            {
                Placa = "LAP-042",
                TipoVehiculo = "Camioneta",
                HoraEntrada = DateTime.Parse("2024-01-15T06:20:00"),
                HoraSalida = DateTime.Parse("2024-01-15T06:50:00"),
                EspacioAsignado = "B-12",
                NumeroReasignaciones = 0,
                TiempoEmbarqueMinutos = 30
            },
            new()
            {
                Placa = "SIN-123",
                TipoVehiculo = "Camión",
                HoraEntrada = DateTime.Parse("2024-01-15T07:00:00"),
                HoraSalida = DateTime.Parse("2024-01-15T07:40:00"),
                EspacioAsignado = "C-07",
                NumeroReasignaciones = 1,
                TiempoEmbarqueMinutos = 40
            }
        }
    };
}
