using HealthApi.Services;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.RenderTree;
using Xunit;

namespace HealthApi.Tests;

public class HealthServiceTests
{
    private readonly HealthService _service;

    public HealthServiceTests()
    {
        _service = new HealthService();
    }

    // [Fact] - Атрибут теста (один сценарий, один результат) - инвариантное условие
    // Формула идеального теста: Fact + Имя-описание + AAA

    [Fact]
    public async Task GetServiceStatuAsync_ExistingService_ReturnsHealthy()
    {
        //Arrange
        var serviceName = "database";
        var ct = CancellationToken.None;

        //Act
        var result = await _service.GetServiceStatusAsync(serviceName, ct);

        //Assert
        Assert.Equal(serviceName, result.ServiceName);
        Assert.True(result.IsHealthy);
    }

    [Fact]
    public async Task GetServiceStatusAsync_NonExistingService_ReturnUnhealthy()
    {
        //Arrange
        var serviceName = "unknown_service";
        var ct = CancellationToken.None;

        //Act
        var result = await _service.GetServiceStatusAsync(serviceName, ct);

        //Assert
        Assert.Equal(serviceName, result.ServiceName);
        Assert.False(result.IsHealthy);
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
}