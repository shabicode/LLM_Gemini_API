using pruieba.Models;
using pruieba.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configurar GeminiSettings desde appsettings.json
builder.Services.Configure<GeminiSettings>(options =>
{
    var geminiSection = builder.Configuration.GetSection("Gemini");
    geminiSection.Bind(options);

    // Leer API Key desde variable de entorno si está configurada
    var envApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
    if (!string.IsNullOrEmpty(envApiKey))
    {
        options.ApiKey = envApiKey;
    }
});

// Registrar HttpClient con IGeminiService
builder.Services.AddHttpClient<IGeminiService, GeminiService>();

// Add services to the container.
builder.Services.AddControllers();

// Configurar Swagger/OpenAPI con documentación completa
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "🚢 LogiFleet API - Google Gemini Integration",
        Version = "v1.0",
        Description = @"
**API REST para integración con Google Gemini AI**

Sistema de control logístico para TRANSPORTES PREMIUM S.A.
Ruta: Mazatlán ↔ La Paz

### 🎯 Funcionalidades:
- **Chat Conversacional**: Asistente virtual para operadores portuarios
- **Análisis de Imágenes**: OCR y visión artificial para documentos vehiculares
- **Generación de Reportes**: Reportes ejecutivos, detección de anomalías y análisis operativos

### 🔑 Configuración:
1. Configura tu API Key en la variable de entorno `GEMINI_API_KEY`
2. O edita `appsettings.Development.json`

### 📚 Documentación completa:
Ver archivo `README_GEMINI.md` en el repositorio
",
        Contact = new OpenApiContact
        {
            Name = "Soporte LogiFleet",
            Email = "soporte@transportespremium.com.mx"
        }
    });

    // Incluir comentarios XML si existen
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Configurar ejemplos de esquemas
    options.EnableAnnotations();
});

// Configurar CORS si es necesario
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "LogiFleet API v1");
        options.DocumentTitle = "LogiFleet - Gemini Integration";
        options.RoutePrefix = "swagger";

        // Configuración visual
        options.DefaultModelsExpandDepth(2);
        options.DefaultModelExpandDepth(2);
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
        options.DisplayRequestDuration();
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Página de bienvenida en la raíz
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
