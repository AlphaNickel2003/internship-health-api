using HealthApi.Models;
using HealthApi.DTOs;

namespace HealthApi.Services;

public interface IHealthAnalyticsService
{
    Task<IEnumerable<HealthRecord>> GetHealthyServicesAsync(IEnumerable<HealthRecord> records, CancellationToken ct);
    Task<IEnumerable<string>> GetServiceNamesAsync(IEnumerable<HealthRecord> records, CancellationToken ct);
    Task<HealthRecord?> GetFirstUnhealthyServiceAsync(IEnumerable<HealthRecord> records, CancellationToken ct);
    Task<HealthStats> GetHealthStatsAsync(IEnumerable<HealthRecord> records, CancellationToken ct);
    Task<IEnumerable<ServiceCheckCount>> GetServicesWithCheckCountAsync(IEnumerable<HealthRecord> records, CancellationToken ct);
    Task<IEnumerable<ServiceTrend>> GetHealthTrendAsync(IEnumerable<HealthRecord> records, CancellationToken ct);
    Task<IEnumerable<HealthRecord>> GetFilteredServicesAsync(
        IEnumerable<HealthRecord> records,
        string? name,
        bool? isHealthy,
        int page,
        int pageSize,
        CancellationToken ct);
}