# ============================================
# Scripts Rápidos para LogiFleet API
# ============================================

# 1. CONFIGURAR API KEY Y EJECUTAR
Write-Host "🔑 Configurando Gemini API Key..." -ForegroundColor Cyan
$env:GEMINI_API_KEY = "AIzaSyCWTXlPCOM6id4dqI0CsvCIQVyuUXkr6gE"

Write-Host "🚀 Iniciando LogiFleet API..." -ForegroundColor Green
dotnet run

# La aplicación estará disponible en:
# https://localhost:5001
# https://localhost:5001/swagger
