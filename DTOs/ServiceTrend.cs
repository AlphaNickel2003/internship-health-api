namespace HealthApi.DTOs;

/// <summary>
/// DTO: тренд здоровья сервисва (Stable/Improved/Degraded)
/// </summary>
public record ServiceTrend(string ServiceName, string Trend);