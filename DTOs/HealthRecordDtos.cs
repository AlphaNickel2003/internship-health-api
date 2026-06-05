namespace HealthApi.DTOs;

public record CreateHealthRecordDto(
    string ServiceName,
    bool IsHealthy,
    DateTime CheckedAt
);

public record UpdateHealthStatusDto(
    bool IsHealthy
);

public record HealthRecordResponseDto(
    int Id,
    string ServiceName,
    bool IsHealthy,
    DateTime CheckedAt
);