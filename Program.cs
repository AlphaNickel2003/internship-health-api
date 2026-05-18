using HealthApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IHealthService, HealthService>();
builder.Services.AddScoped<IHealthAnalyticsService, HealthAnalyticsService>();

var app = builder.Build();

app.MapControllers();       

app.Run();