
# Health API

**Health API** — это учебный RESTful веб-сервис на платформе ASP.NET Core для мониторинга состояния (health checks) различных сервисов в системе. Проект позволяет добавлять записи о проверках, отслеживать статус, получать аналитику и фильтровать историю проверок.

> **Статус проекта:** В разработке

## Основные возможности

- **CRUD операции** с записями о состоянии сервисов (создание, чтение, обновление, удаление).
- **Получение актуального статуса** конкретного сервиса или всех сервисов.
- **Общая проверка здоровья приложения** (OK / DEGRADED).
- **Аналитика и статистика:**
  - Количество здоровых/нездоровых сервисов.
  - Детальная статистика по каждому сервису (количество проверок, процент здоровья).
  - Список сервисов с количеством проверок.
  - Тренд здоровья сервиса (Stable / Improved / Degraded).
- **Фильтрация и пагинация** записей по имени и статусу.
- **Поиск первого нездорового сервиса.**

## Технологический стек

- **.NET 9.0** (Web API)
- **Entity Framework Core 8.0.0** (ORM)
- **PostgreSQL** (база данных, через Npgsql)
- **Swagger / OpenAPI** (документация API)
- **xUnit + Moq** (юнит-тестирование)
- **C# 12.0**

## Структура проекта


```
HealthApi/
├── Controllers/               # Контроллеры API
│   ├── HealthController.cs    # Основные CRUD и статусы
│   └── HealthAnalyticsController.cs # Аналитика и фильтрация
├── Data/                      # Контекст БД
│   └── AppDbContext.cs
├── DTOs/                      # Объекты передачи данных
│   ├── HealthRecordDtos.cs
│   ├── ServiceCheckCount.cs
│   └── ServiceTrend.cs
├── Models/                    # Сущности БД
│   ├── HealthRecord.cs
│   └── HealthStats.cs
├── Services/                  # Бизнес-логика
│   ├── IHealthService.cs
│   ├── HealthService.cs       # CRUD и работа с БД
│   ├── IHealthAnalyticsService.cs
│   └── HealthAnalyticsService.cs # Аналитические операции
├── Migrations/                # Миграции EF Core
├── HealthApi.Tests/           # Юнит-тесты (xUnit)
├── Properties/
├── Program.cs                 # Точка входа и настройка DI
├── appsettings.json           # Конфигурация
└── HealthApi.csproj
```

## Установка и запуск

### Требования

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- PostgreSQL (локально или в Docker)

### Шаги

1. **Клонировать репозиторий**
   ```bash
   git clone https://github.com/your-username/HealthApi.git
   cd HealthApi
   ```

2. **Настроить строку подключения**
   Отредактируй `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=HealthDb;Username=postgres;Password=your_password"
   }
   ```

3. **Применить миграции**
   ```bash
   dotnet ef database update
   ```

4. **Запустить проект**
   ```bash
   dotnet run
   ```
   API будет доступно по адресу: `http://localhost:5006` (или `https://localhost:5001`).

5. **Документация Swagger**
   Открой браузер: `http://localhost:5006/swagger`

## API Эндпоинты

Все эндпоинты доступны по базовому пути `/Health` и `/HealthAnalytics`.

### HealthController

| Метод | URL | Описание |
|-------|-----|----------|
| `GET` | `/Health` | Общий статус приложения (OK / DEGRADED) |
| `GET` | `/Health/all` | Статусы всех сервисов |
| `GET` | `/Health/{serviceName}` | Статус конкретного сервиса (возвращает 503, если нездоров) |
| `GET` | `/Health/{id}` | Получить запись по ID |
| `POST` | `/Health` | Добавить новую запись |
| `PUT` | `/Health/{id}` | Обновить статус записи |
| `DELETE` | `/Health/{id}` | Удалить запись |

### HealthAnalyticsController

| Метод | URL | Описание |
|-------|-----|----------|
| `GET` | `/HealthAnalytics/filter` | Фильтрация записей с пагинацией (query-параметры: `name`, `isHealthy`, `page`, `pageSize`) |
| `GET` | `/HealthAnalytics/healthy` | Список только здоровых сервисов |
| `GET` | `/HealthAnalytics/stats` | Общая статистика по проверкам |
| `GET` | `/HealthAnalytics/first-unhealthy` | Первый нездоровый сервис |
| `GET` | `/HealthAnalytics/check-counts` | Количество проверок по каждому сервису |
| `GET` | `/HealthAnalytics/trends` | Тренды здоровья сервисов |

## Тестирование

В проекте есть набор юнит-тестов для сервисного слоя с использованием in-memory базы данных.

Запустить тесты можно командой:

```bash
dotnet test
```

## Планы по развитию

- Добавить поддержку SignalR для real-time обновлений статусов.
- Интеграция с системой логирования (Serilog).
- Docker-контейнеризация (Dockerfile + docker-compose).
- Расширить аналитику: графики, среднее время восстановления.
- Добавить аутентификацию и авторизацию (JWT).
- Настроить CI/CD (GitHub Actions).

## Как помочь

Проект учебный, но я буду рад любым идеям, замечаниям или pull request'ам! Если хочешь предложить улучшение, создавай issue или пиши напрямую.
