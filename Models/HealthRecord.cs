namespace HealthApi.Models;

public record HealthRecord(
    string ServiceName,
    bool IsHealthy,
    DateTime CheckedAt);

    