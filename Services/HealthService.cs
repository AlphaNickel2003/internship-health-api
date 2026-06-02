using HealthApi.Data;
using HealthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthApi.Services;

public class HealthService : IHealthService
{
    private readonly AppDbContext _context;

    public HealthService(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<HealthRecord?> GetServiceStatusAsync(
        string serviceName,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        return await _context.HealthRecords
            .Where(r => r.ServiceName == serviceName)
            .OrderByDescending(r => r.CheckedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<HealthRecord>> GetAllServiceStatusAsync(
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        return await _context.HealthRecords
            .OrderByDescending(r => r.CheckedAt)
            .ToListAsync(ct);
    }

    public async Task<HealthRecord> AddHealthRecordAsync(HealthRecord record, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        _context.HealthRecords.Add(record);
        await _context.SaveChangesAsync(ct);

        return record;
    }
}   