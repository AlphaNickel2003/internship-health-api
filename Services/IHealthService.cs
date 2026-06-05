using HealthApi.Models;
using HealthApi.DTOs;

namespace HealthApi.Services;

public interface IHealthService
{
    Task<HealthRecord?> GetServiceStatusAsync(string serviceName, CancellationToken ct);

    //IEnumerable позволяет вернуть List, Dictionary, Array без конвертации
    Task<IEnumerable<HealthRecord>> GetAllServiceStatusAsync(CancellationToken ct);

    Task<HealthRecord> AddAsync(CreateHealthRecordDto dto, CancellationToken ct);

    Task<bool> UpdateStatusAsync(int id, UpdateHealthStatusDto dto, CancellationToken ct);

    Task<bool> DeleteAsync(int id, CancellationToken ct);

    Task<HealthRecord?> GetByIdAsync(int id, CancellationToken ct);
}