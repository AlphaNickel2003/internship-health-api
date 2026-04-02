using HealthApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IHealthService, HealthService>();

var app = builder.Build();

app.MapControllers();       

app.Run();