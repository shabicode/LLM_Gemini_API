# 🚢 LogiFleet - Integración con Google Gemini

Sistema de control logístico para **TRANSPORTES PREMIUM S.A.** con inteligencia artificial powered by Google Gemini.

---

## 📋 Requisitos Previos

- **.NET 8 SDK** instalado
- **API Key de Google AI Studio** ([Obtener aquí](https://makersuite.google.com/app/apikey))
- **Visual Studio 2022+** o **VS Code** con extensión C#

---

## 🔧 Configuración Inicial

### 1. Configurar API Key

#### Opción A: Variable de Entorno (Recomendado)
```bash
# Windows PowerShell
$env:GEMINI_API_KEY="TU_API_KEY_AQUI"

# Linux/Mac
export GEMINI_API_KEY="TU_API_KEY_AQUI"
```

#### Opción B: Archivo appsettings.Development.json
```json
{
  "Gemini": {
    "ApiKey": "TU_API_KEY_AQUI"
  }
}
```

⚠️ **IMPORTANTE**: Nunca subas la API Key al repositorio. Está en `.gitignore`.

### 2. Restaurar Paquetes
```bash
dotnet restore
```

### 3. Ejecutar la Aplicación
```bash
dotnet run
```

La API estará disponible en: `https://localhost:5001` (o el puerto configurado)

---

## 🎯 Endpoints Disponibles

### 1️⃣ **Test de Conexión**
```http
GET /api/gemini/test
```
**Respuesta:**
```json
{
  "success": true,
  "response": "✅ Conexión exitosa con Gemini..."
}
```

---

### 2️⃣ **Chat Conversacional**
```http
POST /api/gemini/chat
Content-Type: application/json

{
  "message": "¿Cómo registro la entrada de un camión de carga?",
  "history": []
}
```

**Caso de Uso LogiFleet:**
- Asistencia a operadores portuarios
- Consultas sobre procedimientos de embarque
- Ayuda con asignación de espacios en cubiertas

---

### 3️⃣ **Análisis de Imágenes (Visión Artificial)**
```http
POST /api/gemini/analyze
Content-Type: application/json

{
  "imageBase64": "iVBORw0KGgoAAAANSUhEUgAA...",
  "prompt": "Analiza esta imagen de un documento vehicular. Extrae: número de placa, tipo de vehículo, nombre del propietario y fecha de vencimiento. Devuelve en formato JSON."
}
```

**Caso de Uso LogiFleet:**
- Extracción automática de datos de tarjetas de circulación
- Validación de documentos de vehículos
- OCR de placas vehiculares

---

### 4️⃣ **Generación de Reportes**
```http
POST /api/gemini/report
Content-Type: application/json

{
  "fecha": "2024-01-15",
  "totalVehiculos": 87,
  "tipoReporte": "resumen",
  "movimientos": [
    {
      "placa": "ABC-123",
      "tipoVehiculo": "Automóvil",
      "horaEntrada": "2024-01-15T08:30:00",
      "horaSalida": "2024-01-15T09:15:00",
      "espacioAsignado": "A-12",
      "numeroReasignaciones": 0,
      "tiempoEmbarqueMinutos": 45
    }
  ]
}
```

**Tipos de Reporte:**
- `resumen`: Resumen ejecutivo de operaciones diarias
- `anomalias`: Detección de patrones inusuales
- `ejecutivo`: Reporte profesional para gerencia

---

## 📊 Ejemplos de Uso en LogiFleet

### Asistente Virtual para Operadores
```csharp
// Prompt optimizado para el contexto del ferry
var chatRequest = new ChatRequest
{
    Message = "Un camión de 12 metros necesita embarcar ¿en qué cubierta lo asigno?",
    History = new List<ChatMessage>
    {
        new() { Role = "user", Message = "Eres un experto en logística portuaria..." },
        new() { Role = "model", Message = "Entendido, te ayudaré con operaciones del ferry." }
    }
};
```

### Detección de Anomalías Operativas
```csharp
// Detectar vehículos con tiempo excesivo de embarque
var reportRequest = new ReportRequest
{
    Fecha = DateTime.Today.ToString("yyyy-MM-dd"),
    TipoReporte = "anomalias",
    Movimientos = operacionesDelDia.ToList()
};
```

### Extracción de Datos de Documentos
```csharp
// Analizar imagen de tarjeta de circulación
var analyzeRequest = new AnalyzeImageRequest
{
    ImageBase64 = Convert.ToBase64String(imageBytes),
    Prompt = "Extrae placa, tipo de vehículo y vigencia en formato JSON."
};
```

---

## 🧪 Pruebas con Swagger

Accede a la documentación interactiva en:
```
https://localhost:5001/swagger
```

Desde ahí puedes probar todos los endpoints sin necesidad de Postman.

---

## ⚙️ Configuración Avanzada

Edita `appsettings.json` para ajustar parámetros:

```json
{
  "Gemini": {
    "ModelId": "gemini-2.0-flash",
    "MaxOutputTokens": 2048,
    "Temperature": 0.7,
    "TimeoutSeconds": 30
  }
}
```

| Parámetro | Descripción | Rango |
|-----------|-------------|-------|
| `Temperature` | Creatividad de las respuestas | 0.0 - 2.0 |
| `MaxOutputTokens` | Longitud máxima de respuesta | 100 - 8192 |
| `TimeoutSeconds` | Timeout de peticiones HTTP | 10 - 120 |

---

## 🔒 Seguridad

✅ **Buenas Prácticas Implementadas:**
- API Key almacenada en variables de entorno
- Validación de entrada en todos los endpoints
- Manejo de excepciones con logging detallado
- Timeout configurado para evitar peticiones infinitas
- Sin exposición de datos sensibles en logs

---

## 📚 Arquitectura del Proyecto

```
pruieba/
├── Controllers/
│   └── GeminiController.cs      # Endpoints REST
├── Services/
│   ├── IGeminiService.cs        # Interfaz del servicio
│   └── GeminiService.cs         # Implementación con HttpClient
├── Models/
│   ├── GeminiSettings.cs        # Configuración
│   ├── GeminiRequest.cs         # DTOs de petición
│   ├── GeminiResponse.cs        # DTOs de respuesta
│   └── GeminiDtos.cs            # DTOs de endpoints
├── Program.cs                   # Configuración DI y Swagger
└── appsettings.json             # Configuración de la app
```

---

## 🐛 Solución de Problemas

### Error: "API Key inválida"
- Verifica que `GEMINI_API_KEY` esté configurada correctamente
- Comprueba que la key esté activa en Google AI Studio

### Error: "Timeout"
- Aumenta `TimeoutSeconds` en `appsettings.json`
- Verifica tu conexión a internet

### Error: "No candidates in response"
- El modelo bloqueó la respuesta por contenido inapropiado
- Revisa el prompt y evita términos problemáticos

---

## 📞 Contacto

**Proyecto:** LogiFleet  
**Cliente:** TRANSPORTES PREMIUM S.A.  
**Ruta:** Mazatlán ↔ La Paz  

---

## 📝 Licencia

Este proyecto es propietario de TRANSPORTES PREMIUM S.A.

---

**Desarrollado con ❤️ usando .NET 8 y Google Gemini AI**
