using IpBlock.Middleware;
using IpBlock.Repositories;
using IpBlock.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ICountryRepository, CountryRepository>();
builder.Services.AddSingleton<ICountryService, CountryService>();

builder.Services.AddHostedService<TemporalBlockCleanupService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "BlockedCountries API", Version = "v1" }));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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