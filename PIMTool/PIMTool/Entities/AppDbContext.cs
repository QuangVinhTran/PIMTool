using Microsoft.EntityFrameworkCore;

namespace PIMTool.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }
    public DbSet<GroupEntity> Groups { get; set; }
    public DbSet<ProjectEmployeeEntity> ProjectEmployees { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjectEmployeeEntity>()
            .HasKey(pe => new {pe.ProjectId, pe.EmployeeId});
    }

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var stringConnection = configuration["ConnectionStrings:DefaultConnectionStrings"];
        return stringConnection == null? "" : stringConnection;
    }
}