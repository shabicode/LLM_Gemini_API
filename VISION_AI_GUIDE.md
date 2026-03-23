# 📸 Análisis de Imágenes con Google Gemini

## ✅ **SOPORTE COMPLETO CON SDK 1.4.0**

La versión 1.4.0 del SDK `Google.GenAI` incluye soporte completo para análisis de imágenes mediante `InlineData` y `FileData`.

---

## 🎯 **Cómo Funciona**

### **Estructura del Request**

```csharp
var content = new GeminiTypes.Content
{
    Role = "user",
    Parts = new List<GeminiTypes.Part>
    {
        // Parte 1: Texto (prompt)
        new GeminiTypes.Part { Text = "Describe lo que ves en esta imagen" },
        
        // Parte 2: Imagen (FileData)
        new GeminiTypes.Part
        {
            FileData = new GeminiTypes.FileData
            {
                MimeType = "image/jpeg",
                FileUri = $"data:image/jpeg;base64,{base64Image}"
            }
        }
    }
};
```

---

## 📋 **Endpoint de Análisis de Imágenes**

### **POST /api/gemini/analyze**

**Request Body:**
```json
{
  "imageBase64": "iVBORw0KGgoAAAANSUhEUg...",
  "prompt": "Analiza esta imagen de una tarjeta de circulación. Extrae: placa, tipo de vehículo, propietario y vigencia. Devuelve en formato JSON."
}
```

**Response:**
```json
{
  "success": true,
  "response": "{\n  \"placa\": \"ABC-1234\",\n  \"tipoVehiculo\": \"Automóvil\",\n  \"propietario\": \"Juan Pérez\",\n  \"vigencia\": \"2025-12-31\"\n}",
  "errorMessage": null,
  "tokensUsed": null
}
```

---

## 🖼️ **Casos de Uso en LogiFleet**

### **1. OCR de Tarjetas de Circulación**

**Prompt:**
```
Analiza esta imagen de una tarjeta de circulación vehicular mexicana.
Extrae la siguiente información y devuélvela en formato JSON:

{
  "placa": "XXX-XXX",
  "tipoVehiculo": "Automóvil/Camión/Motocicleta",
  "marca": "Marca del vehículo",
  "modelo": "Año del modelo",
  "propietario": "Nombre completo",
  "vigencia": "YYYY-MM-DD",
  "niv": "Número de Identificación Vehicular"
}

Si algún campo no es visible, usa null.
```

---

### **2. Inspección Visual de Áreas de Carga**

**Prompt:**
```
Analiza esta fotografía del área de carga del ferry.

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

Genera un reporte breve para el supervisor.
```

---

### **3. Validación de Documentos**

**Prompt:**
```
Verifica si esta imagen muestra un documento vehicular válido.

Valida:
1. ¿Es una tarjeta de circulación legítima?
2. ¿Los datos son legibles?
3. ¿La vigencia está actualizada?
4. ¿Hay señales de alteración o falsificación?

Responde: VÁLIDO, SOSPECHOSO o ILEGIBLE
Justifica tu respuesta brevemente.
```

---

## 🔧 **Cómo Obtener una Imagen en Base64**

### **Opción 1: PowerShell**
```powershell
$imageBytes = [System.IO.File]::ReadAllBytes("C:\ruta\imagen.jpg")
$base64 = [Convert]::ToBase64String($imageBytes)
Write-Output $base64
```

### **Opción 2: JavaScript (Frontend)**
```javascript
const fileInput = document.getElementById('imageInput');
const file = fileInput.files[0];
const reader = new FileReader();

reader.onloadend = function() {
    const base64String = reader.result.split(',')[1];
    console.log(base64String);
};

reader.readAsDataURL(file);
```

### **Opción 3: Python**
```python
import base64

with open("imagen.jpg", "rb") as image_file:
    base64_string = base64.b64encode(image_file.read()).decode('utf-8')
    print(base64_string)
```

---

## 📊 **Formatos de Imagen Soportados**

| Formato | MIME Type | Extensión | Soporte |
|---------|-----------|-----------|---------|
| JPEG | `image/jpeg` | `.jpg`, `.jpeg` | ✅ Completo |
| PNG | `image/png` | `.png` | ✅ Completo |
| WebP | `image/webp` | `.webp` | ✅ Completo |
| HEIC | `image/heic` | `.heic` | ✅ Completo |
| HEIF | `image/heif` | `.heif` | ✅ Completo |

---

## 🧪 **Ejemplo Completo en C#**

```csharp
public async Task<string> AnalizarTarjetaCirculacion(string rutaImagen)
{
    // 1. Leer imagen y convertir a Base64
    var imageBytes = await File.ReadAllBytesAsync(rutaImagen);
    var base64Image = Convert.ToBase64String(imageBytes);
    
    // 2. Crear el request
    var analyzeRequest = new AnalyzeImageRequest
    {
        ImageBase64 = base64Image,
        Prompt = @"Analiza esta tarjeta de circulación. 
                   Extrae: placa, tipo de vehículo, propietario y vigencia.
                   Devuelve en formato JSON."
    };
    
    // 3. Enviar al endpoint
    var httpClient = new HttpClient();
    var json = JsonSerializer.Serialize(analyzeRequest);
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    
    var response = await httpClient.PostAsync(
        "https://localhost:5001/api/gemini/analyze",
        content
    );
    
    // 4. Procesar respuesta
    var responseJson = await response.Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<GeminiApiResponse>(responseJson);
    
    return result.Response;
}
```

---

## ⚙️ **Configuración Avanzada**

### **Tamaño Máximo de Imagen**

El SDK tiene límites de tamaño. Para imágenes grandes:

```csharp
// Redimensionar imagen antes de convertir a Base64
using var image = Image.Load(rutaImagen);
image.Mutate(x => x.Resize(new ResizeOptions
{
    Size = new Size(1024, 768),
    Mode = ResizeMode.Max
}));

using var ms = new MemoryStream();
image.SaveAsJpeg(ms, new JpegEncoder { Quality = 85 });
var base64 = Convert.ToBase64String(ms.ToArray());
```

### **Múltiples Imágenes**

```csharp
var parts = new List<GeminiTypes.Part>
{
    new() { Text = "Compara estas dos imágenes:" },
    new() { FileData = new() { MimeType = "image/jpeg", FileUri = $"data:image/jpeg;base64,{image1}" } },
    new() { FileData = new() { MimeType = "image/jpeg", FileUri = $"data:image/jpeg;base64,{image2}" } }
};
```

---

## 🐛 **Solución de Problemas**

### **Error: "Image too large"**
**Solución:** Redimensiona la imagen a máximo 2048x2048 px

### **Error: "Invalid base64"**
**Solución:** Elimina el prefijo `data:image/...;base64,` si existe

### **Respuesta vacía**
**Solución:** Verifica que la imagen sea legible y el prompt sea claro

---

## 📈 **Mejores Prácticas**

1. **Calidad de Imagen:**
   - Resolución mínima: 640x480
   - Formato recomendado: JPEG (menor tamaño)
   - Calidad JPEG: 80-90%

2. **Prompts Efectivos:**
   - Sé específico sobre qué extraer
   - Pide formato estructurado (JSON)
   - Incluye ejemplos si es necesario

3. **Manejo de Errores:**
   - Valida Base64 antes de enviar
   - Implementa retry logic
   - Registra errores para debugging

---

## ✅ **Checklist de Implementación**

- [x] SDK Google.GenAI 1.4.0 instalado
- [x] Endpoint `/api/gemini/analyze` implementado
- [x] Soporte para FileData configurado
- [x] Validación de formato de imagen
- [x] Manejo de errores robusto
- [x] Logging implementado
- [x] Documentación en Swagger
- [x] Ejemplos de uso creados

---

**Desarrollado con ❤️ usando .NET 8 y Google Gemini AI**
