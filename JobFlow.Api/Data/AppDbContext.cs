using JobFlowApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JobFlowApi.Data;

public class AppDbContext : DbContext{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 2. Tell EF Core to save this specific enum as words (text)
        modelBuilder.Entity<User>()
            .Property(a => a.Role)
            .HasConversion<string>(); 
    }

    public DbSet<Company> Companies {get; set;}

    public DbSet<Job> Jobs {get; set;}

    public DbSet<User> Users {get; set;}
}