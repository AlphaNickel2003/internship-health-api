using HealthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    //DbSet - таблица в БД
    public DbSet<HealthRecord> HealthRecords { get; set; } = null!;

    //Настройка схемы
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Индекс для быстрого поиска по ServiceName
        modelBuilder.Entity<HealthRecord>()
            .HasIndex(r => r.ServiceName);

        //Инициализация строковых полей
        modelBuilder.Entity<HealthRecord>()
            .Property(r => r.ServiceName)
            .IsRequired()
            .HasMaxLength(100);
    }
}