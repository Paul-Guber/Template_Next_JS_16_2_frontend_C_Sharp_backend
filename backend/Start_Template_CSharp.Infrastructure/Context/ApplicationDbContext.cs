using Microsoft.EntityFrameworkCore;
using Start_Template_CSharp.Core.Entities;

namespace Start_Template_CSharp.Infrastructure.Context;

public class ApplicationDbContext : DbContext 
{
    public DbSet<EmployeeEntity>  Employees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Ищем ключ "CONNECTION_DB" из файла .env 
        string connectionString = Environment.GetEnvironmentVariable("CONNECTION_DB") ?? "";
        optionsBuilder.UseSqlServer(@connectionString, b
            => b.MigrationsAssembly("Start_Template_CSharp.Infrastructure"))
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
    }
}