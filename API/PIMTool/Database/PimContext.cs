using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;

namespace PIMTool.Database
{
    public class PimContext : DbContext
    {
        // TODO: Define your models
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; } = null!;

        public PimContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(e =>
            {
                e.ToTable("Employees");
                e.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Group>(e =>
            {
                e.ToTable("Groups");
                e.HasKey(e => e.Id);
                e.HasMany(g => g.Employees)
                    .WithOne(e => e.Group)
                    .HasForeignKey(e => e.GroupId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Project>(e =>
            {
                e.ToTable("Projects");
                e.HasKey(e => e.Id);

                e.HasOne(p => p.Group)
                    .WithMany(g => g.Projects)
                    .HasForeignKey(p => p.GroupId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProjectEmployee>(e =>
            {
                e.ToTable("ProjectEmployees");
                e.HasKey(pe => new { pe.ProjectId, pe.EmployeeId });

                e.HasOne(pe => pe.Project)
                    .WithMany(p => p.ProjectEmployees)
                    .HasForeignKey(pe => pe.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(pe => pe.Employee)
                    .WithMany(e => e.ProjectEmployees)
                    .HasForeignKey(pe => pe.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}