using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using System.Configuration;

namespace PIMTool.Database
{
    public class PimContext : DbContext
    {
        public PimContext(DbContextOptions options) : base(options)
        {
        }

        // TODO: Define your models
        public DbSet<Project> Projects { get; set; } = null!;

        public DbSet<Employee> Employees { get; set; } = null!;

        public DbSet<Group> Groups { get; set; } = null!;

        public DbSet<ProjectEmployee> ProjectEmployees { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Group)
                .WithMany(g => g.Projects)
                .HasForeignKey(p => p.GroupId);

            modelBuilder.Entity<Project>()
                .HasIndex(p => p.ProjectNumber)
                .IsUnique();

            modelBuilder.Entity<Group>()
                .HasOne(e => e.GroupLeader)
                .WithOne(g => g.Group)
                .HasForeignKey<Group>(g => g.GroupLeaderId);

            modelBuilder.Entity<Employee>()
                .Property(e => e.BirthDate)
                .HasColumnType("date");

            modelBuilder.Entity<ProjectEmployee>()
                .HasKey(pe => new { pe.ProjectId, pe.EmployeeId });

            modelBuilder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Project)
                .WithMany(p => p.ProjectEmployees)
                .HasForeignKey(pe => pe.ProjectId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Employee)
                .WithMany(e => e.ProjectEmployees)
                .HasForeignKey(pe => pe.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }

    }
}