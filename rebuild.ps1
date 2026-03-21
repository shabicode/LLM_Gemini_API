# ============================================
# Script para Detener y Recompilar LogiFleet
# ============================================

Write-Host "🛑 Deteniendo procesos de pruieba..." -ForegroundColor Yellow

# Detener todos los procesos de pruieba/dotnet que puedan estar bloqueando
Get-Process -Name "pruieba" -ErrorAction SilentlyContinue | Stop-Process -Force
Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Where-Object { $_.MainWindowTitle -like "*pruieba*" } | Stop-Process -Force

Start-Sleep -Seconds 2

Write-Host "✅ Procesos detenidos" -ForegroundColor Green
Write-Host ""
Write-Host "🔨 Recompilando proyecto..." -ForegroundColor Cyan

dotnet clean
dotnet build

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "✅ Compilación exitosa!" -ForegroundColor Green
    Write-Host ""
    Write-Host "¿Deseas iniciar la aplicación? (S/N): " -NoNewline -ForegroundColor Yellow
    $respuesta = Read-Host
    
    if ($respuesta -eq "S" -or $respuesta -eq "s") {
        Write-Host ""
        Write-Host "🚀 Iniciando LogiFleet API..." -ForegroundColor Green
        Write-Host "📖 Swagger estará disponible en: https://localhost:5001/swagger" -ForegroundColor Cyan
        Write-Host ""
        
        # Configurar API Key si no está configurada
        if (-not $env:GEMINI_API_KEY) {
            $env:GEMINI_API_KEY = "AIzaSyCWTXlPCOM6id4dqI0CsvCIQVyuUXkr6gE"
            Write-Host "🔑 API Key configurada desde appsettings.json" -ForegroundColor Yellow
        }
        
        dotnet run
    }
} else {
    Write-Host ""
    Write-Host "❌ Error en la compilación. Revisa los errores arriba." -ForegroundColor Red
}
