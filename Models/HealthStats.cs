namespace HealthApi.Models;

public record HealthStats(
    int Total,
    int Healthy,
    int Unhealthy,
    DateTime LastCheck,
    IEnumerable<ServiceGroupStats> ByService);

public record ServiceGroupStats(
    string Name,
    int Checks,
    bool AllHealthy,
    double HealthPercentage);