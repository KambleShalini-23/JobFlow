using JobFlowApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JobFlowApi.Data;

public class AppDbContext : DbContext{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Company> Companies {get; set;}

    public DbSet<Job> Jobs {get; set;}

    public DbSet<User> Users {get; set;}
}