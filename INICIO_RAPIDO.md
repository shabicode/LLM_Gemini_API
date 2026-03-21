# 🚀 INICIO RÁPIDO - LogiFleet API con Swagger

## ✅ TODO ESTÁ LISTO

La integración con Google Gemini está completamente configurada y compilada.

---

## 📖 OPCIÓN 1: Usar Swagger UI (Recomendado)

### Paso 1: Iniciar la aplicación
```powershell
# Opción A: Script automático
.\start.ps1

# Opción B: Manual
$env:GEMINI_API_KEY = "AIzaSyCWTXlPCOM6id4dqI0CsvCIQVyuUXkr6gE"
dotnet run
```

### Paso 2: Abrir Swagger en el navegador
```
https://localhost:5001/swagger
```

**O simplemente:**
```
https://localhost:5001
```
(Redirige automáticamente a Swagger)

### Paso 3: Probar los endpoints
1. Haz clic en cualquier endpoint (ej: `GET /api/gemini/test`)
2. Clic en **"Try it out"**
3. Clic en **"Execute"**
4. ¡Listo! Verás la respuesta de Gemini

---

## 🖥️ OPCIÓN 2: Panel HTML de Pruebas

### Abrir directamente en el navegador
```powershell
# Abrir el panel de testing
start test-panel.html
```

Este panel tiene:
- ✅ Verificación de estado de la API
- ✅ Botones para probar cada endpoint
- ✅ Visualización de respuestas en tiempo real
- ✅ Ejemplos de código pre-configurados

---

## 🧪 ENDPOINTS DISPONIBLES

### 1️⃣ Test de Conexión
```
GET https://localhost:5001/api/gemini/test
```
**Sin parámetros. Simplemente ejecuta y verifica la respuesta.**

---

### 2️⃣ Chat Conversacional
```
POST https://localhost:5001/api/gemini/chat
```

**Body de ejemplo:**
```json
{
  "message": "¿Cómo registro un camión de 12 metros?",
  "history": []
}
```

**En Swagger:**
1. Haz clic en `POST /api/gemini/chat`
2. Clic en "Try it out"
3. Edita el JSON (o usa el ejemplo)
4. Clic en "Execute"

---

### 3️⃣ Análisis de Imágenes
```
POST https://localhost:5001/api/gemini/analyze
```

**Body de ejemplo:**
```json
{
  "imageBase64": "iVBORw0KGgoAAAANSUhEUg...",
  "prompt": "Extrae la placa, tipo de vehículo y propietario de esta imagen"
}
```

**Para obtener una imagen en Base64:**
```powershell
$bytes = [System.IO.File]::ReadAllBytes("C:\ruta\imagen.jpg")
$base64 = [Convert]::ToBase64String($bytes)
Write-Output $base64
```

---

### 4️⃣ Generación de Reportes
```
POST https://localhost:5001/api/gemini/report
```

**Body de ejemplo (Resumen):**
```json
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

**Tipos de reporte disponibles:**
- `"resumen"` - Resumen ejecutivo de operaciones
- `"anomalias"` - Detecta patrones inusuales
- `"ejecutivo"` - Reporte profesional para gerencia

---

## 🎯 FLUJO DE TESTING RECOMENDADO

### Test Básico (1 minuto)
```
1. dotnet run
2. Abrir https://localhost:5001/swagger
3. GET /api/gemini/test → "Try it out" → "Execute"
4. ✅ Verificar respuesta exitosa
```

### Test Completo (5 minutos)
```
1. Test de conexión (GET /test)
2. Chat simple (POST /chat con history vacío)
3. Chat con contexto (POST /chat con history)
4. Reporte básico (POST /report tipo "resumen")
5. Detección de anomalías (POST /report tipo "anomalias")
```

---

## 🛠️ SCRIPTS ÚTILES

### Recompilar si hay errores
```powershell
.\rebuild.ps1
```
Este script:
- ✅ Detiene procesos bloqueados
- ✅ Limpia y recompila el proyecto
- ✅ Pregunta si quieres iniciar la app

### Iniciar con configuración automática
```powershell
.\start.ps1
```
Este script:
- ✅ Configura la API Key automáticamente
- ✅ Inicia la aplicación
- ✅ Muestra las URLs de acceso

---

## 📊 EJEMPLOS PRE-CONFIGURADOS EN SWAGGER

Swagger incluye ejemplos para cada endpoint:

### Chat - Pregunta sobre operaciones
```json
{
  "message": "¿Cómo registro un camión de 12 metros?",
  "history": []
}
```

### Reporte - Detección de anomalías
```json
{
  "fecha": "2024-01-15",
  "tipoReporte": "anomalias",
  "movimientos": [
    {
      "placa": "XYZ-789",
      "tiempoEmbarqueMinutos": 65,
      "numeroReasignaciones": 3,
      "horaEntrada": "2024-01-15T23:45:00"
    }
  ]
}
```

---

## ⚙️ CONFIGURACIÓN AVANZADA

### Cambiar temperatura (creatividad)
En `appsettings.json`:
```json
{
  "Gemini": {
    "Temperature": 0.9
  }
}
```
- **0.0-0.5**: Respuestas más precisas y consistentes
- **0.5-1.0**: Balance entre creatividad y precisión
- **1.0-2.0**: Respuestas más creativas y variadas

### Aumentar longitud de respuestas
```json
{
  "Gemini": {
    "MaxOutputTokens": 4096
  }
}
```

### Aumentar timeout
```json
{
  "Gemini": {
    "TimeoutSeconds": 60
  }
}
```

---

## 🐛 SOLUCIÓN DE PROBLEMAS

### Error: "Proceso bloqueado al compilar"
**Solución:**
```powershell
.\rebuild.ps1
```

### Error: "API Key inválida"
**Solución:** Ya está configurada en `appsettings.json`. 
Si quieres usar una variable de entorno:
```powershell
$env:GEMINI_API_KEY = "TU_API_KEY"
dotnet run
```

### Error: "Puerto 5001 en uso"
**Solución:**
```powershell
# Ver procesos usando el puerto
netstat -ano | findstr :5001

# Detener el proceso
taskkill /F /PID [NUMERO_PID]
```

### Error 500 en Swagger
**Causa:** Problema con la API de Gemini

**Verificar:**
1. Logs en la terminal donde ejecutaste `dotnet run`
2. API Key válida
3. Conexión a internet

---

## 📚 DOCUMENTACIÓN COMPLETA

- **README_GEMINI.md** - Documentación técnica completa
- **GUIA_SWAGGER.md** - Guía detallada de Swagger con más ejemplos
- **PRUEBAS_GEMINI.md** - Scripts PowerShell para testing
- **Examples/GeminiUsageExamples.cs** - 8 ejemplos de código C#

---

## 🎉 ¡LISTO PARA USAR!

**Ahora ejecuta:**
```powershell
dotnet run
```

**Y abre en tu navegador:**
```
https://localhost:5001/swagger
```

**Características implementadas:**
✅ 4 endpoints funcionales  
✅ Swagger UI interactivo  
✅ Documentación completa  
✅ Ejemplos pre-configurados  
✅ Manejo de errores robusto  
✅ Logging detallado  
✅ Seguridad (API Key protegida)  

---

**🚢 LogiFleet | TRANSPORTES PREMIUM S.A.**  
**Powered by Google Gemini AI**
