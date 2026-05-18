using HealthApi.Models;

namespace HealthApi.Services;

public class HealthAnalyticsService : IHealthAnalyticsService
{
    public Task<IEnumerable<HealthRecord>> GetHealthyServicesAsync(
        IEnumerable<HealthRecord> records,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        //LINQ: Фильтрация по состоянию здоровья
        var healthy = records
            .Where(r => r.IsHealthy)
            .ToList();

        return Task.FromResult<IEnumerable<HealthRecord>>(healthy);
    }

    public Task<IEnumerable<string>> GetServiceNamesAsync(
        IEnumerable<HealthRecord> records,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        //LINQ: Проекция (список имен сервисов)
        var names = records
            .Select(r => r.ServiceName)
            .ToList();

        return Task.FromResult<IEnumerable<string>>(names);
    }

    public Task<HealthRecord?> GetFirstUnhealthyServiceAsync(
        IEnumerable<HealthRecord> records,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        //LINQ: Поиск первого нездорового сервиса
        var firstUnhealthy = records
            .FirstOrDefault(r => !r.IsHealthy);
        
        return Task.FromResult<HealthRecord?>(firstUnhealthy);
    }

    public Task<bool> GetAllServicesHealthyAsync(
        IEnumerable<HealthRecord> records, 
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        //LINQ: Проверка общего состояния здоровья всех сервисов
        var allHealthy = records
            .All(r => r.IsHealthy);

        return Task.FromResult<bool>(allHealthy);
    }

    public Task<HealthStats> GetHealthStatsAsync(
        IEnumerable<HealthRecord> records, 
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        //Сбор статистики
        var listStats = records.ToList();
        
        var total = listStats.Count;
        var healthy = listStats.Count(r => r.IsHealthy);
        var unhealthy = total - healthy;
        var lastCheck = listStats.Max(r => r.CheckedAt);

        //LINQ: Группировка
        var byService = listStats
            .GroupBy(r => r.ServiceName)
            .Select(g => new ServiceGroupStats(
                Name: g.Key,
                Checks: g.Count(),
                AllHealthy: g.All(r => r.IsHealthy),
                HealthPercentage: g.Count(r => r.IsHealthy) * 100.0 / g.Count()
            ))
            .ToList();

        var stats = new HealthStats(
            Total: total,
            Healthy: healthy,
            Unhealthy: unhealthy,
            LastCheck: lastCheck,
            ByService: byService);

        return Task.FromResult(stats);
    }
}