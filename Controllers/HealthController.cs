using Microsoft.AspNetCore.Mvc;
using HealthApi.Services;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using HealthApi.DTOs;

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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var record = await _healthService.GetByIdAsync(id, ct);
        if (record == null) return NotFound();

        var dto = new HealthRecordResponseDto(
            record.Id, 
            record.ServiceName, 
            record.IsHealthy, 
            record.CheckedAt);
            return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> AddRecord(
        [FromBody] CreateHealthRecordDto dto, 
        CancellationToken ct)
    {
        var createdEntity = await _healthService.AddAsync(dto, ct);

        var responseDto = new HealthRecordResponseDto(
            createdEntity.Id,
            createdEntity.ServiceName,
            createdEntity.IsHealthy,
            createdEntity.CheckedAt
        );

        return CreatedAtAction(
            nameof(GetById), 
            new { id = responseDto.Id}, 
            responseDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRecordStatus(
        int id, 
        [FromBody] UpdateHealthStatusDto dto, 
        CancellationToken ct)
    {
        var isUpdated = await _healthService.UpdateStatusAsync(id, dto, ct);
        if (!isUpdated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRecord(int id, CancellationToken ct)
    {
        var isDeleted = await _healthService.DeleteAsync(id, ct);
        if (!isDeleted) return NotFound();
        return NoContent();
    }
}