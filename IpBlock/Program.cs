using IpBlock.Middleware;
using IpBlock.Options;
using IpBlock.Repositories;
using IpBlock.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add configuration options
builder.Services.Configure<IpApiOptions>(builder.Configuration.GetSection("IpApi"));
builder.Services.Configure<TemporalBlockCleanupOptions>(builder.Configuration.GetSection("TemporalBlockCleanup"));

// HttpClient for IP API
builder.Services.AddHttpClient("IpApi");

// Repositories
builder.Services.AddSingleton<ICountryRepository, CountryRepository>();
builder.Services.AddSingleton<ILogRepository, LogRepository>();

// Services
builder.Services.AddSingleton<ICountryService, CountryService>();
builder.Services.AddSingleton<ILogService, LogService>();
builder.Services.AddSingleton<IIpApiService, IpApiService>();
builder.Services.AddHostedService<TemporalBlockCleanupService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blocked Countries API", Version = "v1" });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
