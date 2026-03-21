# Scripts de Prueba para la Integración con Gemini

## Configurar Variable de Entorno (PowerShell)
```powershell
$env:GEMINI_API_KEY="TU_API_KEY_AQUI"
```

## Test 1: Verificar Conexión
```powershell
curl -X GET "https://localhost:5001/api/gemini/test" -k
```

## Test 2: Chat Simple
```powershell
$body = @{
    message = "¿Cómo embarco un vehículo de 10 metros?"
    history = @()
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/gemini/chat" `
    -Method POST `
    -Body $body `
    -ContentType "application/json" `
    -SkipCertificateCheck
```

## Test 3: Análisis de Imagen (ejemplo con imagen de prueba)
```powershell
# Convertir imagen local a Base64
$imageBytes = [System.IO.File]::ReadAllBytes("C:\ruta\imagen.jpg")
$base64Image = [Convert]::ToBase64String($imageBytes)

$body = @{
    imageBase64 = $base64Image
    prompt = "Describe lo que ves en esta imagen"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/gemini/analyze" `
    -Method POST `
    -Body $body `
    -ContentType "application/json" `
    -SkipCertificateCheck
```

## Test 4: Generar Reporte de Operaciones
```powershell
$body = @{
    fecha = "2024-01-15"
    totalVehiculos = 87
    tipoReporte = "resumen"
    movimientos = @(
        @{
            placa = "ABC-123"
            tipoVehiculo = "Automóvil"
            horaEntrada = "2024-01-15T08:30:00"
            horaSalida = "2024-01-15T09:15:00"
            espacioAsignado = "A-12"
            numeroReasignaciones = 0
            tiempoEmbarqueMinutos = 45
        },
        @{
            placa = "XYZ-789"
            tipoVehiculo = "Camión"
            horaEntrada = "2024-01-15T09:00:00"
            horaSalida = "2024-01-15T10:30:00"
            espacioAsignado = "C-05"
            numeroReasignaciones = 2
            tiempoEmbarqueMinutos = 90
        }
    )
} | ConvertTo-Json -Depth 10

Invoke-RestMethod -Uri "https://localhost:5001/api/gemini/report" `
    -Method POST `
    -Body $body `
    -ContentType "application/json" `
    -SkipCertificateCheck
```

## Test 5: Detección de Anomalías
```powershell
$body = @{
    fecha = "2024-01-15"
    totalVehiculos = 45
    tipoReporte = "anomalias"
    movimientos = @(
        @{
            placa = "DEF-456"
            tipoVehiculo = "Automóvil"
            horaEntrada = "2024-01-15T23:45:00"  # Fuera de horario
            horaSalida = "2024-01-16T00:30:00"
            espacioAsignado = "B-08"
            numeroReasignaciones = 3              # Muchas reasignaciones
            tiempoEmbarqueMinutos = 65            # Tiempo excesivo
        }
    )
} | ConvertTo-Json -Depth 10

Invoke-RestMethod -Uri "https://localhost:5001/api/gemini/report" `
    -Method POST `
    -Body $body `
    -ContentType "application/json" `
    -SkipCertificateCheck
```

## Monitorear Logs en Tiempo Real
```powershell
dotnet run | Select-String "Gemini"
```

## Verificar Configuración
```powershell
dotnet user-secrets list
```
