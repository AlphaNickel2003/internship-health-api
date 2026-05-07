using Microsoft.AspNetCore.Mvc;
using HealthApi.Services;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace HealthApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    private readonly IHealthService _healthService;

    public HealthController(IHealthService healthService)
    {
        _healthService = healthService;
    }

    //Эндпоинт - статусы всех сервисов
    [HttpGet("all")]
    public async Task<IActionResult> GetAllServices(CancellationToken ct)
    {
        var records = await _healthService.GetAllServiceStatusAsync(ct);
        return Ok(records);
    }

    //Эндпоинт - статус конкретного сервиса
    [HttpGet("{serviceName}")]
    public async Task<IActionResult> GetServiceStatus(string serviceName, CancellationToken ct)
    {
        var record = await _healthService.GetServiceStatusAsync(serviceName, ct);

        //Console.WriteLine($"[Debug] Service: {serviceName}, IsHealthy: {record.IsHealthy}");

        //Если сервис не работает - возвращается 503 (Service Unavailable)
        if(!record.IsHealthy)
        {
            return StatusCode(503, record);
        }

        return Ok(record);
    }

    //Эндпоинт - проверка общего статуса приложения
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var all = await _healthService.GetAllServiceStatusAsync(ct);
        var overallHealthy = all.All(r => r.IsHealthy);

        return Ok(new { status = overallHealthy ? "OK" : "DEGRADED", checkedAt = DateTime.UtcNow });
    }
}