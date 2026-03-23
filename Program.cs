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
});
 
builder.Services.AddSingleton<IGeminiService, GeminiService>(); 
builder.Services.AddControllers();
 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = " LogiFleet API - Google Gemini Integration",
        Version = "v1.0",
        Contact = new OpenApiContact
        {
            Name = "Soporte LogiFleet", 
        }
    });
     
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
     
    options.EnableAnnotations();
});
 
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
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "LogiFleet API v1");
        options.DocumentTitle = "LogiFleet - Gemini Integration";
        options.RoutePrefix = "swagger";
         
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

// Endpoint de redirección a Swagger (excluido de la documentación)
app.MapGet("/", () => Results.Redirect("/swagger"))
   .ExcludeFromDescription();

app.Run();
