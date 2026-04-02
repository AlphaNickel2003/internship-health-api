namespace HealthApi.Services;

public class HealthService : IHealthService
{
    public Task<string> GetStatusAsync()
    {
        return Task.FromResult($"OK: {DateTime.Now:HH:MM:SS}");
    }
}