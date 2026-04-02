namespace HealthApi.Services;

public interface IHealthService
{
    Task<string> GetStatusAsync();
}