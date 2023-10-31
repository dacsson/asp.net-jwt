using Microsoft.EntityFrameworkCore;
using SimbirGO.Models;

namespace SimbirGO.Contexts;

public class SimbirDbContext : DbContext
{
    /// Таблица профилей пользователей
    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }

    public SimbirDbContext(DbContextOptions<SimbirDbContext> opt) : base(opt) { }

    /// <summary>
    /// Генерирует таблицы в базе данных
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>();
        modelBuilder.Entity<User>().Property<long>(nameof(User.Id)).ValueGeneratedOnAdd();
        modelBuilder.Entity<User>().Property(nameof(User.Username));
        modelBuilder.Entity<User>().Property(nameof(User.PasswordHash));
        modelBuilder.Entity<User>().Property(nameof(User.RoleOfUser));

        modelBuilder.Entity<Vehicle>();
        modelBuilder.Entity<Vehicle>().Property<long>(nameof(Vehicle.Id)).ValueGeneratedOnAdd();
        modelBuilder.Entity<Vehicle>().Property(nameof(Vehicle.Name));
        modelBuilder.Entity<Vehicle>().Property(nameof(Vehicle.RentCost));
        modelBuilder.Entity<Vehicle>().Property(nameof(Vehicle.IsTaken));
        modelBuilder.Entity<Vehicle>().Property(nameof(Vehicle.TypeOfVehicle));
    }
}
