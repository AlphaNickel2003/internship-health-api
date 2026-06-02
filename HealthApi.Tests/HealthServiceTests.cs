using HealthApi.Services;
using HealthApi.Models;
using Microsoft.EntityFrameworkCore;
using HealthApi.Data;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.RenderTree;
using Xunit;

namespace HealthApi.Tests;

public class HealthServiceTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly HealthService _service;
    private readonly CancellationToken _ct = CancellationToken.None;

    public HealthServiceTests()
    {
        // In-memory БД для тестов
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new AppDbContext(options);
        _service = new HealthService(_context);
        SeedTestData();
    }

    private void SeedTestData()
    {
        _context.HealthRecords.AddRange(
            new HealthRecord{ ServiceName ="Database", IsHealthy = true, CheckedAt = DateTime.UtcNow.AddMinutes(-5) },
            new HealthRecord{ ServiceName ="Cache", IsHealthy = false, CheckedAt = DateTime.UtcNow },
            new HealthRecord{ ServiceName ="API", IsHealthy = true, CheckedAt = DateTime.UtcNow.AddMinutes(-5) }
        );

        _context.SaveChanges();
    }

    // [Fact] - Атрибут теста (один сценарий, один результат) - инвариантное условие
    // Формула идеального теста: Fact + Имя-описание + AAA

    [Fact]
    public async Task GetServiceStatusAsync_ExistingService_ReturnsRecord()
    {
        //Arrange
        var serviceName = "Database";
        var ct = CancellationToken.None;

        //Act
        var result = await _service.GetServiceStatusAsync(serviceName, ct);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(serviceName, result.ServiceName);
        Assert.True(result.IsHealthy);
    }

    [Fact]
    public async Task GetServiceStatusAsync_NonExistingService_ReturnUnhealthy()
    {
        var serviceName = "NonExistent";
        var ct = CancellationToken.None;

        var result = await _service.GetServiceStatusAsync(serviceName, ct);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllServiceStatusAsync_ReturnsAllRecords()
    {
        //Arrange
        var ct = CancellationToken.None;

        //Act
        var result = await _service.GetAllServiceStatusAsync(ct);
        var list = result.ToList();

        //Assert
        Assert.Equal(3, list.Count);
        Assert.All(list, r => Assert.NotNull(r.ServiceName));
    }

    [Fact]
    public async Task GetServiceStatusAsync_CaseInsensitive_KeyLookup()
    {
        //Arrange
        var ct = CancellationToken.None;
        
        //Act
        var result1 = await _service.GetServiceStatusAsync("DATABASE", ct);
        var result2 = await _service.GetServiceStatusAsync("database", ct);

        //Assert
        Assert.Equal(result1, result2);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}