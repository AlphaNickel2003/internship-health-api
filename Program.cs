using HealthApi.Services;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddOpenApi();

builder.Services.AddScoped<IHealthService, HealthService>();

var app = builder.Build();

app.Run();