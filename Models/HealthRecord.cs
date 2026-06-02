namespace HealthApi.Models;

/// <summary>
/// Сущность БД: запись о проверке здоровья сервиса
/// </summary>
public class HealthRecord{
    // Первичный ключ
    public int Id { get; set; }

    // Название сервиса (быстрый поиск)
    public string ServiceName { get; set; } = string.Empty;

    //Статус здоровья
    public bool IsHealthy { get; set; }

    //Время проверки
    public DateTime CheckedAt { get; set; }
}

    