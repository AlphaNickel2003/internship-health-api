using System;
using System.Threading.Tasks;
using HealthApi.DTOs;
using HealthApi.Models;
using HealthApi.Services;
using HealthApi.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HealthApi.Tests;

public class  HealthServiceCrudTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly HealthService _service;
    private readonly string _dbName;
    private readonly CancellationToken ct = CancellationToken.None;

    public HealthServiceCrudTests()
    {
        _dbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(_dbName)
            .Options;

        _context = new AppDbContext(options);
        _service = new HealthService(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task AddAsync_ShouldAddRecordAndReturnGeneraredId()
    {
        var dto = new CreateHealthRecordDto("Database", true, DateTime.UtcNow);

        var result = await _service.AddAsync(dto, ct);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("Database", result.ServiceName);
        Assert.True(result.IsHealthy);
    }

    [Fact]
    public async Task UpdateStatusAsync_WhenRecordExists_ShouldReturnTrueAndUpdateStatus()
    {
        var existingRecord = new HealthRecord{ 
            ServiceName = "Cache", 
            IsHealthy = false, 
            CheckedAt = DateTime.UtcNow };

        _context.HealthRecords.Add(existingRecord);
        await _context.SaveChangesAsync();

        var updateDto = new UpdateHealthStatusDto(true);

        var result = await _service.UpdateStatusAsync(existingRecord.Id, updateDto, ct);

        Assert.True(result);
        var updatedRecord = await _context.HealthRecords.FindAsync(existingRecord.Id);
        Assert.True(updatedRecord!.IsHealthy);
    }

    [Fact]
    public async Task UpdateStatusAsync_WhenRecordNotFound_ShouldReturnFalse()
    {
        var updateDto = new UpdateHealthStatusDto(true);
        var result = await _service.UpdateStatusAsync(999, updateDto, ct);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRecordromDatabase()
    {
        var dto = new CreateHealthRecordDto(
            ServiceName: "Message", 
            IsHealthy: true, 
            CheckedAt: DateTime.UtcNow);

        var record = await _service.AddAsync(dto, ct);
        var result = await _service.DeleteAsync(record.Id, ct);

        Assert.True(result);
        var deletedRecord = await _context.HealthRecords.FindAsync(record.Id);
        Assert.Null(deletedRecord);
    }
}