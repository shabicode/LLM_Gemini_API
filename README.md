# LogiFleet API - Google Gemini Integration

Sistema de control logístico para TRANSPORTES PREMIUM S.A. con inteligencia artificial integrada mediante Google Gemini.

---

## Descripción del Proyecto

LogiFleet es una API REST diseñada para optimizar las operaciones del ferry en la ruta Mazatlán - La Paz, utilizando IA generativa para:

- Asistente Virtual para operadores portuarios
- Generación de reportes ejecutivos y operativos
- Detección de anomalías en operaciones
- Análisis de imágenes (OCR de documentos vehiculares)

---

## Inicio Rápido

### Visual Studio (Recomendado)

1. Abre el proyecto en Visual Studio 2022+
2. Presiona F5 o haz clic en el botón de ejecución
3. El navegador se abrirá automáticamente en Swagger

### PowerShell/Terminal
 
```powershell
dotnet run
```

Swagger estará disponible en: **https://localhost:5001/swagger**

---

## Endpoints Disponibles

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/gemini/test` | Verificar conexión con Gemini |
| `POST` | `/api/gemini/chat` | Chat conversacional con IA |
| `POST` | `/api/gemini/analyze` | Análisis de imágenes con visión artificial |
| `POST` | `/api/gemini/report` | Generación de reportes operativos |

---

## Requisitos

- .NET 8 SDK ([Descargar](https://dotnet.microsoft.com/download/dotnet/8.0))
- Visual Studio 2022+ o VS Code
- API Key de Google AI Studio ([Obtener aquí](https://makersuite.google.com/app/apikey))

---

## Configuración

### 1. API Key

La API Key está configurada en `appsettings.json`:

```json
{
  "Gemini": {
    "ApiKey": "TU_API_KEY_AQUI"
  }
}
```

**IMPORTANTE**: Nunca subas la API Key al repositorio público. Agrega `appsettings.json` a `.gitignore` o usa Azure Key Vault para producción.

### 2. Configuración del Modelo

Edita `appsettings.json` para ajustar parámetros:

```json
{
  "Gemini": {
    "ApiKey": "YOUR_API_KEY",
    "ModelId": "gemini-2.5-flash",
    "BaseUrl": "https://generativelanguage.googleapis.com/v1beta/models",
    "MaxOutputTokens": 2048,
    "Temperature": 0.7,
    "TimeoutSeconds": 30
  }
}
```

**Parámetros disponibles:**

| Parámetro | Descripción | Valores |
|-----------|-------------|---------|
| `ModelId` | Modelo de Gemini a utilizar | gemini-2.5-flash, gemini-1.5-pro, etc. |
| `Temperature` | Creatividad de las respuestas | 0.0 - 2.0 (0.7 recomendado) |
| `MaxOutputTokens` | Longitud máxima de respuesta | 100 - 8192 |
| `TimeoutSeconds` | Timeout de peticiones HTTP | 10 - 120 |

---

## Documentación

| Archivo | Descripción |
|---------|-------------|
| [README_GEMINI.md](README_GEMINI.md) | Documentación técnica de la integración |
| [GUIA_SWAGGER.md](GUIA_SWAGGER.md) | Tutorial completo de Swagger UI |
| [VISION_AI_GUIDE.md](VISION_AI_GUIDE.md) | Guía de análisis de imágenes |
| [Examples/](Examples/) | Ejemplos de código en C# |

---

## Testing

### Swagger UI (Interfaz Gráfica)

Accede a Swagger en:
```
https://localhost:5001/swagger
```

Desde Swagger puedes probar todos los endpoints de forma interactiva con ejemplos precargados.

---

## Arquitectura

```
pruieba/
├── Controllers/
│   ├── GeminiController.cs       # Endpoints REST
│   └── SwaggerExamples.cs        # Ejemplos para Swagger
├── Services/
│   ├── IGeminiService.cs         # Interfaz del servicio
│   └── GeminiService.cs          # Implementación con SDK Google.GenAI
├── Models/
│   ├── GeminiSettings.cs         # Configuración
│   ├── GeminiRequest.cs          # DTOs de petición
│   ├── GeminiResponse.cs         # DTOs de respuesta
│   └── GeminiDtos.cs             # DTOs de endpoints
├── Examples/
│   └── GeminiUsageExamples.cs    # Ejemplos de uso
├── Properties/
│   └── launchSettings.json       # Configuración de Visual Studio
├── Program.cs                    # Configuración DI y Swagger
└── appsettings.json              # Configuración de la aplicación
```

---

## Ejemplos de Uso

### Chat Conversacional
```json
POST /api/gemini/chat
{
  "message": "¿Cómo registro un camión de 12 metros?",
  "history": []
}
```

### Análisis de Imagen
```json
POST /api/gemini/analyze
{
  "imageBase64": "iVBORw0KGgo...",
  "prompt": "Extrae la placa y tipo de vehículo"
}
```

### Reporte de Operaciones
```json
POST /api/gemini/report
{
  "fecha": "2024-01-15",
  "tipoReporte": "resumen",
  "totalVehiculos": 87,
  "movimientos": [...]
}
```

Ver [GUIA_SWAGGER.md](GUIA_SWAGGER.md) para ejemplos completos.

---

## Scripts Útiles

| Script | Descripción |
|--------|-------------|
| `.\start.ps1` | Inicia la aplicación |
| `.\rebuild.ps1` | Limpia y recompila el proyecto |

---

## Seguridad

**Implementado:**
- API Key desde archivo de configuración (no hardcodeada)
- Validación de entrada en endpoints
- Manejo de excepciones completo
- Timeout configurado (30s por defecto)
- Logging de operaciones
- CORS configurado

**Recomendaciones para producción:**
- Usar Azure Key Vault o variables de entorno para la API Key
- Implementar rate limiting
- Agregar autenticación/autorización
- Configurar HTTPS obligatorio

---

## Solución de Problemas

### Error: "Proceso bloqueado"
```powershell
.\rebuild.ps1
```

### Error: "API Key inválida"
Verifica que la API Key esté configurada correctamente en `appsettings.json` y sea válida.

### Error: Timeout
Aumenta el valor de `TimeoutSeconds` en `appsettings.json`.

### Error: "Model not found"
Verifica que el `ModelId` en `appsettings.json` sea un modelo válido de Gemini (actualmente: gemini-2.5-flash).

---

## Características

- Integración con Google Gemini 2.5 Flash
- Swagger UI interactivo con ejemplos
- Chat conversacional con historial de contexto
- Análisis de imágenes (visión artificial multimodal)
- Generación de reportes en lenguaje natural
- Detección de anomalías operativas
- Arquitectura de servicios con inyección de dependencias
- Documentación completa
- Scripts de automatización

---

## Stack Tecnológico

- **Backend:** .NET 8 (C#)
- **LLM:** Google Gemini 2.5 Flash
- **SDK:** Google.GenAI v1.4.0 (oficial)
- **Documentación:** Swagger/OpenAPI (Swashbuckle v6.5.0)
- **Testing:** Swagger UI + Scripts PowerShell
- **Patrones:** Dependency Injection, Options Pattern, Service Layer

---
 