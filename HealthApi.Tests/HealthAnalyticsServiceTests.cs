using System.Runtime.CompilerServices;
using HealthApi.Models;
using HealthApi.Services;
using Xunit;

namespace HealthApi.Tests;

public class HealthAnalyticsServiceTests
{
    private readonly HealthAnalyticsService _service;
    private readonly List<HealthRecord> _testRecords;

    public HealthAnalyticsServiceTests()
    {
        _service = new HealthAnalyticsService();
        _testRecords = new List<HealthRecord>
        {
            new HealthRecord("database", true, DateTime.UtcNow),
            new HealthRecord("cache", true, DateTime.UtcNow),
            new HealthRecord("email", false, DateTime.UtcNow),
            new HealthRecord("database", true, DateTime.UtcNow.AddMinutes(-1))
        };
    }

    [Fact]
    public async Task GetHealthyServicesAsync_ReturnOnlyHealthyRecords()
    {
        //Arrange
        var ct = CancellationToken.None;

        //Act
        var result = await _service.GetHealthyServicesAsync(_testRecords, ct);
        var list = result.ToList();

        //Assert
        Assert.Equal(3, list.Count);
        Assert.All(list, r => Assert.True(r.IsHealthy));
    }

    [Fact]
    public async Task GetServiceNamesAsync_ReturnsAllNames()
    {
        var ct = CancellationToken.None;

        var result = await _service.GetServiceNamesAsync(_testRecords, ct);
        var list = result.ToList();

        Assert.Equal(4, list.Count);
        Assert.Contains("database", list);
        Assert.Contains("email", list);
    }

    [Fact]
    public async Task GetFirstUnhealthyServiceAsync_ReturnsFirstUnhealthy()
    {
        var ct = CancellationToken.None;

        var result = await _service.GetFirstUnhealthyServiceAsync(_testRecords, ct);

        Assert.NotNull(result);
        Assert.Equal("email", result.ServiceName);
        Assert.False(result.IsHealthy);
    }

    [Fact]
    public async Task AreAllServicesHealthyAsync_ReturnsFalse_WhenAnyHealthy()
    {
        var ct = CancellationToken.None;

        var result =  await _service.GetAllServicesHealthyAsync(_testRecords, ct);

        Assert.False(result);
    }

    [Fact]
    public async Task GetHealthStatsAsync_ReturnsCorrectStats()
    {
        var ct = CancellationToken.None;

        var result = await _service.GetHealthStatsAsync(_testRecords, ct);

        Assert.Equal(4, result.Total);
        Assert.Equal(3, result.Healthy);
        Assert.Equal(1, result.Unhealthy);
        Assert.NotEqual(default, result.LastCheck);
        Assert.NotEmpty(result.ByService);
    }

    [Fact]
    public async Task GetHealthyServicesAsync_EmptyList_ReturnsEmpty()
    {
        var ct = CancellationToken.None;
        var emptyRecords = new List<HealthRecord>();

        var result = await _service.GetHealthyServicesAsync(emptyRecords, ct);
        var list = result.ToList();

        Assert.Empty(list);
    }
}