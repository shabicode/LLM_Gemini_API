using pruieba.Models;
using pruieba.Services;
using System.Text.Json;

namespace pruieba.Examples;

/// <summary>
/// Ejemplos prácticos de uso de la integración con Gemini en el contexto de LogiFleet.
/// Estos ejemplos muestran cómo usar el servicio en diferentes escenarios operativos.
/// </summary>
public class GeminiUsageExamples
{
    private readonly IGeminiService _geminiService;

    public GeminiUsageExamples(IGeminiService geminiService)
    {
        _geminiService = geminiService;
    }

    /// <summary>
    /// EJEMPLO 1: Asistente Virtual para Operadores del Ferry
    /// Contexto: Un operador necesita ayuda para resolver dudas operativas
    /// </summary>
    public async Task<string> EjemploAsistenteOperadorAsync()
    {
        var promptBase = @"Eres un asistente logístico especializado del sistema LogiFleet 
        para TRANSPORTES PREMIUM S.A. Tu función es ayudar a los operadores portuarios 
        de la ruta Mazatlán-La Paz con:
        - Procedimientos de embarque y desembarque
        - Asignación de espacios en las 3 cubiertas del ferry (A, B, C)
        - Clasificación de vehículos (ligeros: cubiertas A-B, pesados: cubierta C)
        - Resolución de incidencias y conflictos de espacio
        - Normativas de seguridad portuaria
        
        Responde siempre de forma breve, clara y profesional para personal no técnico.";

        var preguntaOperador = "Un autobús de turismo de 14 metros acaba de llegar. " +
                               "¿En qué cubierta debería asignarlo y qué espacio recomendarías?";

        var respuesta = await _geminiService.GenerateTextAsync(
            $"{promptBase}\n\nPregunta del operador: {preguntaOperador}"
        );

        return respuesta;
    }

    /// <summary>
    /// EJEMPLO 2: Conversación Contextual (Chat con Historial)
    /// Contexto: Un operador hace varias preguntas relacionadas en secuencia
    /// </summary>
    public async Task<List<string>> EjemploConversacionContextualAsync()
    {
        var respuestas = new List<string>();

        // Historial de conversación
        var historial = new List<ChatMessage>
        {
            new() { 
                Role = "user", 
                Message = "Eres el asistente de LogiFleet. Ayúdame con operaciones del ferry." 
            },
            new() { 
                Role = "model", 
                Message = "Entendido. Soy tu asistente de operaciones LogiFleet. ¿En qué puedo ayudarte?" 
            }
        };

        // Primera pregunta
        var respuesta1 = await _geminiService.ChatAsync(
            historial,
            "¿Cuál es el procedimiento para embarcar un vehículo con carga peligrosa?"
        );
        respuestas.Add(respuesta1);

        // Agregar al historial
        historial.Add(new ChatMessage { Role = "user", Message = "¿Cuál es el procedimiento..." });
        historial.Add(new ChatMessage { Role = "model", Message = respuesta1 });

        // Segunda pregunta (hace referencia a la anterior)
        var respuesta2 = await _geminiService.ChatAsync(
            historial,
            "¿Y si el vehículo también transporta pasajeros?"
        );
        respuestas.Add(respuesta2);

        return respuestas;
    }

    /// <summary>
    /// EJEMPLO 3: Generación de Reporte Ejecutivo Diario
    /// Contexto: Al final del día, se genera un resumen de operaciones para gerencia
    /// </summary>
    public async Task<string> EjemploReporteEjecutivoAsync()
    {
        var datosOperacion = new
        {
            Fecha = DateTime.Today.ToString("yyyy-MM-dd"),
            Viajes = new[]
            {
                new { NumeroViaje = 1, Salida = "06:00", Llegada = "09:30", VehiculosEmbarcados = 42 },
                new { NumeroViaje = 2, Salida = "11:00", Llegada = "14:30", VehiculosEmbarcados = 38 },
                new { NumeroViaje = 3, Salida = "16:00", Llegada = "19:30", VehiculosEmbarcados = 45 }
            },
            Estadisticas = new
            {
                TotalVehiculos = 125,
                TiempoPromedioEmbarque = 28.5,
                EficienciaOcupacion = 87.3,
                Incidencias = 2
            },
            Incidencias = new[]
            {
                new { Hora = "08:15", Tipo = "Retraso", Descripcion = "Vehículo con documentación incompleta" },
                new { Hora = "17:45", Tipo = "Reasignación", Descripcion = "Cambio de espacio por dimensiones incorrectas" }
            }
        };

        var jsonDatos = JsonSerializer.Serialize(datosOperacion, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        });

        var promptReporte = $@"Analiza los siguientes datos de operación del ferry 
        de TRANSPORTES PREMIUM S.A. y genera un reporte ejecutivo profesional en español 
        con las siguientes secciones:

        1. RESUMEN EJECUTIVO (2-3 oraciones)
        2. MÉTRICAS PRINCIPALES (viajes, vehículos, eficiencia)
        3. ANÁLISIS DE INCIDENCIAS
        4. RECOMENDACIONES OPERATIVAS

        Datos:
        {jsonDatos}";

        return await _geminiService.GenerateTextAsync(promptReporte);
    }

    /// <summary>
    /// EJEMPLO 4: Detección de Anomalías Operativas
    /// Contexto: Identificar patrones inusuales en las operaciones del día
    /// </summary>
    public async Task<string> EjemploDeteccionAnomaliasAsync()
    {
        var movimientos = new[]
        {
            new { Placa = "XYZ-789", TiempoEmbarque = 62, Reasignaciones = 3, Hora = "08:45" },
            new { Placa = "ABC-123", TiempoEmbarque = 15, Reasignaciones = 0, Hora = "09:20" },
            new { Placa = "DEF-456", TiempoEmbarque = 45, Reasignaciones = 2, Hora = "23:30" },
            new { Placa = "GHI-789", TiempoEmbarque = 18, Reasignaciones = 0, Hora = "10:15" }
        };

        var jsonMovimientos = JsonSerializer.Serialize(movimientos, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        });

        var promptAnomalias = $@"Analiza estos movimientos vehiculares del ferry y detecta anomalías:

        CRITERIOS DE ANOMALÍA:
        - Tiempo de embarque > 30 minutos (normal: 15-25 min)
        - Más de 2 reasignaciones de espacio
        - Entradas fuera del horario operativo (06:00-22:00)

        Formato de respuesta:
        🚨 ANOMALÍAS DETECTADAS:
        1. [Placa] - [Tipo de anomalía] - [Detalles]
        
        RECOMENDACIONES:
        - [Lista de acciones correctivas]

        Datos:
        {jsonMovimientos}";

        return await _geminiService.GenerateTextAsync(promptAnomalias);
    }

    /// <summary>
    /// EJEMPLO 5: Análisis de Imagen - Extracción de Datos de Tarjeta de Circulación
    /// Contexto: Digitalización automática de documentos vehiculares al embarcar
    /// </summary>
    public async Task<string> EjemploAnalisisDocumentoVehicularAsync(string imagenBase64)
    {
        var promptVision = @"Analiza esta imagen de una tarjeta de circulación vehicular mexicana.
        Extrae la siguiente información y devuélvela en formato JSON estricto:

        {
          ""placa"": ""XXX-XXX"",
          ""tipoVehiculo"": ""Automóvil/Camión/Motocicleta"",
          ""marca"": ""Marca del vehículo"",
          ""modelo"": ""Año del modelo"",
          ""propietario"": ""Nombre completo"",
          ""vigencia"": ""YYYY-MM-DD"",
          ""niv"": ""Número de Identificación Vehicular""
        }

        Si algún campo no es visible o legible, usa null.
        Si detectas que la imagen no es una tarjeta de circulación, devuelve: {""error"": ""Documento no válido""}";

        return await _geminiService.AnalyzeImageAsync(imagenBase64, promptVision);
    }

    /// <summary>
    /// EJEMPLO 6: Análisis de Imagen - Inspección Visual de Ferry
    /// Contexto: Detectar problemas en fotografías del ferry o áreas de carga
    /// </summary>
    public async Task<string> EjemploInspeccionVisualFerryAsync(string imagenBase64)
    {
        var promptInspeccion = @"Analiza esta fotografía de un área de carga del ferry.
        Identifica:
        
        ✅ ASPECTOS POSITIVOS:
        - Espacios bien aprovechados
        - Vehículos correctamente estacionados
        - Señalización visible
        
        ⚠️ POSIBLES PROBLEMAS:
        - Vehículos mal estacionados
        - Obstrucciones de salidas de emergencia
        - Espacios desperdiciados
        - Daños visibles en infraestructura
        
        Genera un reporte breve y accionable para el supervisor de operaciones.";

        return await _geminiService.AnalyzeImageAsync(imagenBase64, promptInspeccion);
    }

    /// <summary>
    /// EJEMPLO 7: Generación de Respuestas Predefinidas (FAQs Inteligentes)
    /// Contexto: Responder preguntas frecuentes de clientes de forma automática
    /// </summary>
    public async Task<string> EjemploFAQClientesAsync(string preguntaCliente)
    {
        var promptFAQ = $@"Eres un asistente de atención al cliente de TRANSPORTES PREMIUM S.A.
        Responde la siguiente pregunta de un cliente sobre el servicio de ferry Mazatlán-La Paz:

        INFORMACIÓN DEL SERVICIO:
        - Horarios: 3 viajes diarios (06:00, 11:00, 16:00)
        - Duración: 3.5 horas aproximadamente
        - Tarifas: Autos desde $850, Camiones desde $1,200
        - Capacidad: 150 vehículos por viaje
        - Reservaciones: Requeridas con 24h de anticipación
        - Contacto: 668-123-4567 / reservas@transportespremium.com.mx

        Pregunta del cliente: {preguntaCliente}

        Responde de forma amable, profesional y concisa. Si la pregunta no puede ser respondida
        con la información disponible, indica que deben contactar al centro de atención.";

        return await _geminiService.GenerateTextAsync(promptFAQ);
    }

    /// <summary>
    /// EJEMPLO 8: Optimización de Asignación de Espacios con IA
    /// Contexto: Sugerir la mejor distribución de vehículos en las cubiertas
    /// </summary>
    public async Task<string> EjemploOptimizacionEspaciosAsync()
    {
        var vehiculosEnEspera = new[]
        {
            new { Id = 1, Tipo = "Automóvil", Largo = 4.5, Ancho = 1.8, Alto = 1.5 },
            new { Id = 2, Tipo = "Camioneta", Largo = 5.2, Ancho = 2.0, Alto = 2.1 },
            new { Id = 3, Tipo = "Camión", Largo = 12.0, Ancho = 2.5, Alto = 3.8 },
            new { Id = 4, Tipo = "Motocicleta", Largo = 2.1, Ancho = 0.8, Alto = 1.2 },
            new { Id = 5, Tipo = "Autobús", Largo = 14.5, Ancho = 2.6, Alto = 3.5 }
        };

        var espaciosDisponibles = new
        {
            CubiertaA = new { Espacios = 45, AltoMax = 2.0, PesoMax = 2000 },
            CubiertaB = new { Espacios = 35, AltoMax = 2.5, PesoMax = 3500 },
            CubiertaC = new { Espacios = 25, AltoMax = 4.0, PesoMax = 15000 }
        };

        var jsonDatos = JsonSerializer.Serialize(new 
        { 
            VehiculosEnEspera = vehiculosEnEspera, 
            Espacios = espaciosDisponibles 
        }, new JsonSerializerOptions { WriteIndented = true });

        var promptOptimizacion = $@"Como sistema de optimización logística, sugiere la mejor 
        asignación de estos vehículos a las cubiertas del ferry considerando:

        CRITERIOS:
        1. Altura máxima por cubierta
        2. Distribución equilibrada del peso
        3. Maximizar ocupación de espacios
        4. Vehículos pesados siempre en cubierta C
        5. Agrupar vehículos similares para facilitar desembarque

        Devuelve la respuesta en este formato:

        ASIGNACIÓN RECOMENDADA:
        Cubierta A: [Lista de IDs]
        Cubierta B: [Lista de IDs]
        Cubierta C: [Lista de IDs]

        JUSTIFICACIÓN:
        [Breve explicación de la estrategia usada]

        Datos:
        {jsonDatos}";

        return await _geminiService.GenerateTextAsync(promptOptimizacion);
    }
}
