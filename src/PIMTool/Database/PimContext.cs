using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;

namespace PIMTool.Database
{
    public class PimContext : DbContext
    {
        // TODO: Define your models
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Group> Groups { get; set; }

        public PimContext(DbContextOptions<PimContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectEmployee>()
              .HasKey(pe => new { pe.ProjectId, pe.EmployeeId });

            modelBuilder.Entity<Project>()
              .HasOne(p => p.Group)
              .WithMany(g => g.Projects)
              .HasForeignKey(p => p.GroupId);

            modelBuilder.Entity<Group>()
            .HasOne(g => g.GroupLeader)
            .WithMany()
            .HasForeignKey(g => g.GroupLeaderId); // TẠM THỜI SAI VÌ KHÔNG SỬA ĐƯỢC


            modelBuilder.Entity<ProjectEmployee>()
              .HasOne(pe => pe.Project)
              .WithMany(p => p.ProjectEmployees)
              .HasForeignKey(pe => pe.ProjectId);

            modelBuilder.Entity<ProjectEmployee>()
              .HasOne(pe => pe.Employee)
              .WithMany(e => e.ProjectEmployees)
              .HasForeignKey(pe => pe.EmployeeId);
        }

    }
}