using HealthApi.Models;

namespace HealthApi.Services;

public class HealthService : IHealthService
{

    public readonly Dictionary<string, HealthRecord> _services;

    public HealthService()
    {
        _services = new Dictionary<string, HealthRecord>(StringComparer.OrdinalIgnoreCase)
        {
            {"database", new HealthRecord("database", true, DateTime.UtcNow) },
            {"cache", new HealthRecord("cache", true, DateTime.UtcNow) },
            {"email", new HealthRecord("email", false, DateTime.UtcNow) }
        };
    }

    public Task<HealthRecord> GetServiceStatusAsync(
        string serviceName,
        CancellationToken ct)
    {
        //Проверка отмены операции
        ct.ThrowIfCancellationRequested();

        if (_services.TryGetValue(serviceName, out var record))
        {
            return Task.FromResult(record);
        }
        
        //Если не найден - возвращаем фейковую запись со статусом жизни false
        return Task.FromResult( new HealthRecord(serviceName, false, DateTime.UtcNow));
    }

    public Task<IEnumerable<HealthRecord>> GetAllServiceStatusAsync(
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        //Возвращаем все значения из словаря
        return Task.FromResult<IEnumerable<HealthRecord>>(_services.Values);
    }
}   