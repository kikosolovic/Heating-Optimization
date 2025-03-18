using Microsoft.EntityFrameworkCore;

namespace Heating_Optimization.Data;
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=app.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Alice", Password = ("password1") },
            new User { Id = 2, Name = "Bob", Password = ("password2") },
            new User { Id = 3, Name = "Charlie", Password = ("password3") },
            new User { Id = 4, Name = "David", Password = ("password4") },
            new User { Id = 5, Name = "Eve", Password = ("password5") }
        );
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
}
