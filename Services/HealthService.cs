using HealthApi.Data;
using HealthApi.Models;
using HealthApi.DTOs;
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

    public async Task<HealthRecord> AddAsync(CreateHealthRecordDto dto, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var newRecord = new HealthRecord
        {
            ServiceName = dto.ServiceName,
            IsHealthy = dto.IsHealthy,
            CheckedAt = dto.CheckedAt
        };

        await _context.HealthRecords.AddAsync(newRecord, ct);
        await _context.SaveChangesAsync(ct);

        return newRecord;
    }

    public async Task<bool> UpdateStatusAsync(int id, UpdateHealthStatusDto dto, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var record = await _context.HealthRecords.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (record == null) return false;

        record.IsHealthy = dto.IsHealthy;
        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var record = await _context.HealthRecords.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (record == null) return false;

        _context.HealthRecords.Remove(record);
        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<HealthRecord?> GetByIdAsync(int id, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        return await _context.HealthRecords.FindAsync(new object[] {id}, ct);
    }
}   