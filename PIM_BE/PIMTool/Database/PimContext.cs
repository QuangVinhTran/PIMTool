using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;

namespace PIMTool.Database;

public class PimContext : DbContext
{
    public PimContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}