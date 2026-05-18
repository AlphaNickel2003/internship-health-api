using HealthApi.Models;

namespace HealthApi.Services;

public interface IHealthAnalyticsService
{
    Task<IEnumerable<HealthRecord>> GetHealthyServicesAsync(IEnumerable<HealthRecord> records, CancellationToken ct);
    Task<IEnumerable<string>> GetServiceNamesAsync(IEnumerable<HealthRecord> records, CancellationToken ct);
    Task<HealthRecord?> GetFirstUnhealthyServiceAsync(IEnumerable<HealthRecord> records, CancellationToken ct);
    Task<HealthStats> GetHealthStatsAsync(IEnumerable<HealthRecord> records, CancellationToken ct);
}