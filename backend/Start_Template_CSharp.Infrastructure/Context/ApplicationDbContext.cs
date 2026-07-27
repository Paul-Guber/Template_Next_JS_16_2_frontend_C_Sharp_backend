using Microsoft.EntityFrameworkCore;
using Start_Template_CSharp.Core.Entities;

namespace Start_Template_CSharp.Infrastructure.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    

    public DbSet<EmployeeEntity>  Employees { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
    }
}