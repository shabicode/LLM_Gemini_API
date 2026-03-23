# 🚢 LogiFleet API - Google Gemini Integration

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Google Gemini](https://img.shields.io/badge/Google-Gemini%202.0-4285F4?logo=google)](https://ai.google.dev/)
[![License](https://img.shields.io/badge/License-Proprietary-red)](LICENSE)

Sistema de control logístico para **TRANSPORTES PREMIUM S.A.** con inteligencia artificial integrada mediante Google Gemini.

---

## 🎯 Descripción del Proyecto

LogiFleet es una API REST diseñada para optimizar las operaciones del ferry en la ruta **Mazatlán ↔ La Paz**, utilizando IA generativa para:

-  **Asistente Virtual** para operadores portuarios
-  **Generación de reportes** ejecutivos y operativos
-  **Detección de anomalías** en operaciones
-  **Análisis de imágenes** (OCR de documentos vehiculares)

---

## ⚡ Inicio Rápido

### Visual Studio (Recomendado)

1. Abre el proyecto en **Visual Studio 2022+**
2. Presiona **F5** o haz clic en ▶️
3. El navegador se abrirá automáticamente en Swagger

### PowerShell/Terminal

```powershell
# Ejecutar script de inicio
.\start.ps1
```

O manualmente:
```powershell
dotnet run
```

Swagger estará disponible en: **https://localhost:5001/swagger**

---

## 📋 Endpoints Disponibles

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/gemini/test` | Verificar conexión con Gemini |
| `POST` | `/api/gemini/chat` | Chat conversacional con IA |
| `POST` | `/api/gemini/analyze` | Análisis de imágenes con visión artificial |
| `POST` | `/api/gemini/report` | Generación de reportes operativos |

---

##  Requisitos

- **.NET 8 SDK** ([Descargar](https://dotnet.microsoft.com/download/dotnet/8.0))
- **Visual Studio 2022+** o **VS Code**
- **API Key de Google AI Studio** ([Obtener aquí](https://makersuite.google.com/app/apikey))

---

##  Configuración

### 1. API Key

La API Key ya está configurada en `Properties/launchSettings.json` para desarrollo.

**Para producción**, usa variables de entorno:
```powershell
$env:GEMINI_API_KEY = "TU_API_KEY"
```

### 2. Configuración del Modelo

Edita `appsettings.json`:
```json
{
  "Gemini": {
    "ModelId": "gemini-2.0-flash",
    "Temperature": 0.7,
    "MaxOutputTokens": 2048,
    "TimeoutSeconds": 30
  }
}
```

---

##  Documentación

| Archivo | Descripción |
|---------|-------------|
| [INICIO_RAPIDO.md](INICIO_RAPIDO.md) | Guía de inicio rápido paso a paso |
| [GUIA_SWAGGER.md](GUIA_SWAGGER.md) | Tutorial completo de Swagger UI |
| [README_GEMINI.md](README_GEMINI.md) | Documentación técnica de la integración |
| [PRUEBAS_GEMINI.md](PRUEBAS_GEMINI.md) | Scripts PowerShell para testing |
| [Examples/](Examples/) | 8 ejemplos de código en C# |

---

##  Testing

### Swagger UI (Interfaz Gráfica)
```
https://localhost:5001/swagger
```

### Scripts PowerShell
Ver [PRUEBAS_GEMINI.md](PRUEBAS_GEMINI.md)

---

## Arquitectura

```
pruieba/
├── Controllers/
│   ├── GeminiController.cs       # Endpoints REST
│   └── SwaggerExamples.cs        # Ejemplos para Swagger
├── Services/
│   ├── IGeminiService.cs         # Interfaz
│   └── GeminiService.cs          # Implementación con HttpClient
├── Models/
│   ├── GeminiSettings.cs         # Configuración
│   ├── GeminiRequest.cs          # DTOs de petición
│   ├── GeminiResponse.cs         # DTOs de respuesta
│   └── GeminiDtos.cs             # DTOs de endpoints
├── Examples/
│   └── GeminiUsageExamples.cs    # 8 ejemplos de uso
├── Properties/
│   └── launchSettings.json       # Configuración de VS
├── Program.cs                    # Configuración DI y Swagger
└── appsettings.json              # Configuración de la app
```

---

##  Ejemplos de Uso

### Chat Conversacional
```csharp
POST /api/gemini/chat
{
  "message": "¿Cómo registro un camión de 12 metros?",
  "history": []
}
```

### Análisis de Imagen
```csharp
POST /api/gemini/analyze
{
  "imageBase64": "iVBORw0KGgo...",
  "prompt": "Extrae la placa y tipo de vehículo"
}
```

### Reporte de Operaciones
```csharp
POST /api/gemini/report
{
  "fecha": "2024-01-15",
  "tipoReporte": "resumen",
  "totalVehiculos": 87,
  "movimientos": [...]
}
```

---

##  Scripts Útiles

| Script | Descripción |
|--------|-------------|
| `.\start.ps1` | Inicia la aplicación con configuración automática |
| `.\rebuild.ps1` | Limpia y recompila el proyecto |

---

##  Seguridad

 **Implementado:**
- API Key protegida (no hardcodeada)
- Variables de entorno para producción
- `.gitignore` configurado
- Validación de entrada en endpoints
- Manejo de excepciones completo
- Timeout configurado (30s)

---

##  Solución de Problemas

### Error: "Proceso bloqueado"
```powershell
.\rebuild.ps1
```

### Error: "API Key inválida"
Verifica que la key esté configurada en `launchSettings.json` o como variable de entorno.

### Más ayuda
Ver [INICIO_RAPIDO.md](INICIO_RAPIDO.md) sección "Solución de Problemas"

---

##  Características

✅ Integración nativa con Google Gemini 2.0  
✅ Swagger UI interactivo  
✅ Chat conversacional con historial  
✅ Visión artificial (análisis de imágenes)  
✅ Generación de reportes en lenguaje natural  
✅ Detección de anomalías operativas  
✅ Documentación completa  
✅ Ejemplos de código listos para usar  
✅ Scripts de testing automático  
✅ Panel HTML de pruebas  

---

##  Stack Tecnológico

- **Backend:** .NET 8 (C#)
- **LLM:** Google Gemini 2.0 Flash
- **SDK:** Google.GenAI v1.3.0 (oficial)
- **Documentación:** Swagger/OpenAPI
- **Testing:** Swagger UI + Scripts PowerShell

---
