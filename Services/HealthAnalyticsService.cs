using HealthApi.Models;
using HealthApi.DTOs;

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

    public Task<IEnumerable<ServiceCheckCount>> GetServicesWithCheckCountAsync(IEnumerable<HealthRecord> records, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var query = records
            .GroupBy(r => r.ServiceName)
            .Select(g => new ServiceCheckCount(
                ServiceName: g.Key,
                CheckCount: g.Count()))
            .OrderByDescending(x => x.CheckCount);

        var result = query.ToList();

        return Task.FromResult<IEnumerable<ServiceCheckCount>>(result);
    }

    public Task<IEnumerable<ServiceTrend>> GetHealthTrendAsync(IEnumerable<HealthRecord> records, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var trends = records
            .GroupBy(r => r.ServiceName)
            .Select(g =>
            {
                var ordered = g.OrderBy(r => r.CheckedAt).ToList();
                var firstStatus = ordered.First().IsHealthy;
                var lastStatus = ordered.Last().IsHealthy;

                var trend = (firstStatus, lastStatus) switch
                {
                    (true, true) or (false, false) => "Stable",
                    (false, true)                  => "Improved",
                    (true, false)                  => "Degraded"
                };

                return new ServiceTrend(g.Key, trend);
            })
            .ToList();

        return Task.FromResult<IEnumerable<ServiceTrend>>(trends);
    }

    public Task<IEnumerable<HealthRecord>> GetFilteredServicesAsync(
        IEnumerable<HealthRecord> records,
        string? name,
        bool? isHealthy,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var filtered = records;

        if (!string.IsNullOrWhiteSpace(name))
        {
            filtered = filtered.Where(r =>
                r.ServiceName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        if (isHealthy.HasValue)
        {
            filtered = filtered.Where(r =>
                r.IsHealthy == isHealthy.Value);
        }

        filtered = filtered.OrderBy(r => r.ServiceName);

        var safePage = page < 1 ? 1 : page;
        var safePageSize = pageSize < 1 ? 10 : pageSize;

        var skipCount = (safePage - 1) * safePageSize;

        var result = filtered
            .Skip(skipCount)
            .Take(safePageSize)
            .ToList();

        return Task.FromResult<IEnumerable<HealthRecord>>(result);
    }
}