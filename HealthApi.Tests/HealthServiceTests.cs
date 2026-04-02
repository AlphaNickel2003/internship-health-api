using HealthApi.Services;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.RenderTree;
using Xunit;

namespace HealthApi.Tests;

public class HealthServiceTests
{
    [Fact]
    public async Task GetStatusAsync_ReturnStringStartingWithOk()
    {
        // Паттерн AAA - Arrange-Act-Assert

        // Подготовка
        var service = new HealthService();

        // Действие
        var result = await service.GetStatusAsync();

        // Проверка
        Assert.StartsWith("OK", result);
    }

    /// Формула идеального теста: Fact + Имя-описание + AAA
}