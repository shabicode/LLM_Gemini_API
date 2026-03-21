# 🧪 Guía de Testing con Swagger UI

## 🚀 Acceso Rápido

### 1. Ejecutar la aplicación
```powershell
dotnet run
```

### 2. Abrir Swagger en el navegador
```
https://localhost:5001/swagger
```
O simplemente:
```
https://localhost:5001
```
(Redirige automáticamente a Swagger)

---

## 📋 Endpoints Disponibles

### ✅ **1. GET /api/gemini/test**
**Descripción:** Prueba básica de conexión con Gemini

**Cómo probar:**
1. Haz clic en `GET /api/gemini/test`
2. Clic en "Try it out"
3. Clic en "Execute"
4. Verifica que recibas: `✅ Conexión exitosa con Gemini...`

**Resultado esperado:**
```json
{
  "success": true,
  "response": "✅ Conexión exitosa con Gemini. Respuesta: [mensaje del modelo]",
  "errorMessage": null,
  "tokensUsed": null
}
```

---

### 💬 **2. POST /api/gemini/chat**
**Descripción:** Chat conversacional con historial de mensajes

**Ejemplo de Body:**
```json
{
  "message": "¿Cómo registro la entrada de un camión de carga de 12 metros?",
  "history": []
}
```

**Cómo probar:**
1. Haz clic en `POST /api/gemini/chat`
2. Clic en "Try it out"
3. Edita el JSON en el campo "Request body"
4. Clic en "Execute"

**Casos de prueba sugeridos:**

#### Caso 1: Primera interacción
```json
{
  "message": "Hola, necesito ayuda con el embarque de vehículos",
  "history": []
}
```

#### Caso 2: Conversación con contexto
```json
{
  "message": "¿Y si el vehículo mide más de 10 metros?",
  "history": [
    {
      "role": "user",
      "message": "¿En qué cubierta debo asignar un camión de carga?"
    },
    {
      "role": "model",
      "message": "Los camiones de carga deben asignarse en la cubierta C, que está diseñada para vehículos pesados."
    }
  ]
}
```

#### Caso 3: Consulta sobre procedimientos
```json
{
  "message": "¿Cuál es el procedimiento para embarcar un autobús de turismo?",
  "history": []
}
```

---

### 🖼️ **3. POST /api/gemini/analyze**
**Descripción:** Análisis de imágenes con visión artificial

**Ejemplo de Body:**
```json
{
  "imageBase64": "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==",
  "prompt": "Analiza esta imagen de una tarjeta de circulación. Extrae: número de placa, tipo de vehículo, nombre del propietario y fecha de vencimiento. Devuelve en formato JSON."
}
```

**Cómo obtener una imagen en Base64:**

#### Opción A: PowerShell
```powershell
$imageBytes = [System.IO.File]::ReadAllBytes("C:\ruta\imagen.jpg")
$base64 = [Convert]::ToBase64String($imageBytes)
Write-Output $base64
```

#### Opción B: Sitio Web
1. Ve a: https://base64.guru/converter/encode/image
2. Sube tu imagen
3. Copia el código Base64 (sin el prefijo `data:image/...`)

**Casos de prueba sugeridos:**

#### Caso 1: Análisis de documento vehicular
```json
{
  "imageBase64": "[TU_IMAGEN_BASE64]",
  "prompt": "Extrae todos los datos de esta tarjeta de circulación: placa, tipo de vehículo, marca, modelo, propietario y vigencia. Responde en formato JSON."
}
```

#### Caso 2: Análisis de área de carga
```json
{
  "imageBase64": "[TU_IMAGEN_BASE64]",
  "prompt": "Analiza esta fotografía del ferry. Identifica cuántos vehículos están embarcados, si hay espacios mal aprovechados o si detectas algún problema de seguridad."
}
```

---

### 📊 **4. POST /api/gemini/report**
**Descripción:** Generación de reportes operativos

**Tipos de reporte:**
- `"resumen"` - Resumen ejecutivo de operaciones
- `"anomalias"` - Detección de patrones inusuales
- `"ejecutivo"` - Reporte profesional para gerencia

#### **Caso 1: Reporte de Resumen**
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
    },
    {
      "placa": "XYZ-789",
      "tipoVehiculo": "Camión",
      "horaEntrada": "2024-01-15T10:00:00",
      "horaSalida": "2024-01-15T11:00:00",
      "espacioAsignado": "C-05",
      "numeroReasignaciones": 1,
      "tiempoEmbarqueMinutos": 60
    }
  ]
}
```

#### **Caso 2: Detección de Anomalías**
```json
{
  "fecha": "2024-01-15",
  "totalVehiculos": 45,
  "tipoReporte": "anomalias",
  "movimientos": [
    {
      "placa": "DEF-456",
      "tipoVehiculo": "Automóvil",
      "horaEntrada": "2024-01-15T23:45:00",
      "horaSalida": "2024-01-16T00:30:00",
      "espacioAsignado": "B-08",
      "numeroReasignaciones": 3,
      "tiempoEmbarqueMinutos": 65
    },
    {
      "placa": "GHI-789",
      "tipoVehiculo": "Camión",
      "horaEntrada": "2024-01-15T14:00:00",
      "horaSalida": "2024-01-15T15:30:00",
      "espacioAsignado": "C-02",
      "numeroReasignaciones": 5,
      "tiempoEmbarqueMinutos": 90
    }
  ]
}
```

#### **Caso 3: Reporte Ejecutivo**
```json
{
  "fecha": "2024-01-15",
  "totalVehiculos": 156,
  "tipoReporte": "ejecutivo",
  "movimientos": [
    {
      "placa": "MAZ-001",
      "tipoVehiculo": "Automóvil",
      "horaEntrada": "2024-01-15T06:15:00",
      "horaSalida": "2024-01-15T06:45:00",
      "espacioAsignado": "A-01",
      "numeroReasignaciones": 0,
      "tiempoEmbarqueMinutos": 30
    },
    {
      "placa": "LAP-042",
      "tipoVehiculo": "Camioneta",
      "horaEntrada": "2024-01-15T11:00:00",
      "horaSalida": "2024-01-15T11:35:00",
      "espacioAsignado": "B-12",
      "numeroReasignaciones": 0,
      "tiempoEmbarqueMinutos": 35
    }
  ]
}
```

---

## 🎯 Flujo de Testing Completo

### Paso 1: Verificar Configuración
```powershell
# Verificar que la API Key esté configurada
$env:GEMINI_API_KEY
```

### Paso 2: Iniciar Aplicación
```powershell
dotnet run
```

### Paso 3: Probar en Orden
1. ✅ **Test básico** → `GET /api/gemini/test`
2. 💬 **Chat simple** → `POST /api/gemini/chat` (sin historial)
3. 💬 **Chat con contexto** → `POST /api/gemini/chat` (con historial)
4. 📊 **Reporte básico** → `POST /api/gemini/report` (tipo: resumen)
5. 🔍 **Detección anomalías** → `POST /api/gemini/report` (tipo: anomalias)
6. 🖼️ **Análisis imagen** → `POST /api/gemini/analyze` (requiere imagen)

---

## 🐛 Solución de Problemas en Swagger

### Error: "API Key inválida"
**Solución:**
```powershell
$env:GEMINI_API_KEY="TU_API_KEY_REAL"
dotnet run
```

### Error: "Timeout"
**Causa:** La petición tarda más de 30 segundos

**Solución:** Edita `appsettings.json`:
```json
{
  "Gemini": {
    "TimeoutSeconds": 60
  }
}
```

### Error: "Bad Request - Mensaje vacío"
**Causa:** Olvidaste llenar el campo `message` en el JSON

**Solución:** Asegúrate de que el JSON tenga todos los campos requeridos

### Error 500: "Error interno"
**Solución:** Verifica los logs en la terminal donde ejecutaste `dotnet run`

---

## 📝 Tips para Testing Eficiente

### 1. Usar la pestaña "Schemas"
- En Swagger UI, ve a la sección "Schemas" al final
- Ahí puedes ver la estructura completa de cada DTO
- Útil para entender qué campos son requeridos

### 2. Copiar respuestas anteriores
- Las respuestas del modelo se pueden usar como `history` en el siguiente chat
- Ejemplo:
  ```json
  {
    "role": "model",
    "message": "[Copiar respuesta anterior aquí]"
  }
  ```

### 3. Guardar ejemplos favoritos
- Usa la función "Copy as cURL" en Swagger
- Guárdalos en un archivo `.txt` para reutilizarlos

### 4. Validar JSON antes de enviar
- Copia tu JSON en https://jsonlint.com/ para verificar sintaxis
- O usa VSCode con la extensión "JSON Tools"

---

## 🎨 Personalización de Swagger

Si quieres cambiar la apariencia o agregar más documentación, edita `Program.cs`:

```csharp
options.SwaggerDoc("v1", new OpenApiInfo
{
    Title = "Mi Título Personalizado",
    Description = "Mi descripción..."
});
```

---

## 📞 ¿Necesitas Ayuda?

- **Documentación completa:** Ver `README_GEMINI.md`
- **Ejemplos de código:** Ver `Examples/GeminiUsageExamples.cs`
- **Scripts de prueba:** Ver `PRUEBAS_GEMINI.md`

---

**¡Listo para probar! 🚀**
