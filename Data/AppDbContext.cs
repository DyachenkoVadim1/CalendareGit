using CalendareGit.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Начальные данные для тестирования
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin@example.com",
                Password = "Admin123",
                Role = "Admin",
            },
            new User
            {
                Id = 2,
                Username = "user@example.com",
                Password = "User123",
                Role = "User",
            }
        );
    }
}