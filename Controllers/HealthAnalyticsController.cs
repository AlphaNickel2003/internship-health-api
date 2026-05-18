using HealthApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthAnalyticsController : ControllerBase
{
    private readonly IHealthService _healthService;
    private readonly IHealthAnalyticsService _analyticsService;

    public HealthAnalyticsController(IHealthService healthService, IHealthAnalyticsService analyticsService)
    {
        _healthService = healthService;
        _analyticsService = analyticsService;
    }

    [HttpGet("healthy")]
    public async Task<IActionResult> GetHealthyServices(CancellationToken ct)
    {
        var records = await _healthService.GetAllServiceStatusAsync(ct);
        var healthy = await _analyticsService.GetHealthyServicesAsync(records, ct);
        return Ok(healthy);
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats(CancellationToken ct)
    {
        var records = await _healthService.GetAllServiceStatusAsync(ct);
        var stats = await _analyticsService.GetHealthStatsAsync(records, ct);
        return Ok(stats);
    }

    [HttpGet("first-unhealthy")]
    public async Task<IActionResult> GetFirstUnhealthy(CancellationToken ct)
    {
        var records = await _healthService.GetAllServiceStatusAsync(ct);
        var first = await _analyticsService.GetFirstUnhealthyServiceAsync(records, ct);
        
        if (first is null) return Ok(new {message = "All services are healthy"});

        return Ok(first);
    }
}