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

    [HttpGet("filter")]
    public async Task<IActionResult> GetFilteredServices(
        CancellationToken ct,
        [FromQuery] string? name,
        [FromQuery] bool? isHealthy,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (page < 1)
            return BadRequest("Номер страницы не может быть меньше 1.");

        if (pageSize < 1 || pageSize > 100)
            return BadRequest("Размер страницы должен быть от 1 до 100.");

        var records = await _healthService.GetAllServiceStatusAsync(ct);
        var result = await _analyticsService.GetFilteredServicesAsync(
            records, name, isHealthy, page, pageSize, ct);

        if (!result.Any())
        {
            return NotFound("Записи не найдены по заданным фильтрам");
        }

        return Ok(result);
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

    [HttpGet("check-counts")]
    public async Task<IActionResult> GetCheckCounts(CancellationToken ct)
    {
        var records = await _healthService.GetAllServiceStatusAsync(ct);
        var checkCounts = await _analyticsService.GetServicesWithCheckCountAsync(records, ct);
        return Ok(checkCounts);
    }

    [HttpGet("trends")]
    public async Task<IActionResult> GetHealthTrends(CancellationToken ct)
    {
        var records = await _healthService.GetAllServiceStatusAsync(ct);
        var trends = await _analyticsService.GetHealthTrendAsync(records, ct);
        return Ok(trends);
    }
}