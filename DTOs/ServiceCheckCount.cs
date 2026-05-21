namespace HealthApi.DTOs;

/// <summary>
/// DTO: количество проверок
/// </summary>
public record ServiceCheckCount(string ServiceName, int CheckCount);