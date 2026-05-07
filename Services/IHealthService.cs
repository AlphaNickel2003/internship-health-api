using HealthApi.Models;

namespace HealthApi.Services;

public interface IHealthService
{
    Task<HealthRecord> GetServiceStatusAsync(string serviceName, CancellationToken ct);

    //IEnumerable позволяет вернуть List, Dictionary, Array без конвертации
    Task<IEnumerable<HealthRecord>> GetAllServiceStatusAsync(CancellationToken ct);
}