using Microsoft.AspNetCore.Mvc;
using HealthApi.Services;

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

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var status = await _healthService.GetStatusAsync();
        return Ok(status);
    }
}