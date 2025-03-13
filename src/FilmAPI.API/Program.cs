using FilmAPI.API.Middleware;
using FilmAPI.Application.Services;
using FilmAPI.Core.Interfaces;
using FilmAPI.Infrastructure.Data;
using FilmAPI.Infrastructure.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Film API",
        Version = "v1",
        Description = "Film bilgilerini yönetmek için RESTful API"
    });
    
    // XML dosyasını ekle (controller ve model açıklamaları için)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Add memory cache
builder.Services.AddMemoryCache();

// Register services
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Initialize database and seed data
if (app.Environment.IsDevelopment())
{
    await SeedData.Initialize(builder.Configuration);
}

// Configure the HTTP request pipeline.
app.UseExceptionHandling(); // Global hata yönetimi

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Film API v1");
        c.RoutePrefix = string.Empty; // Ana sayfada Swagger'ı göster
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
