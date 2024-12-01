using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures
{
    public class PimContext : DbContext
    {
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; } = null!;

        public PimContext(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mối quan hệ giữa Project và Group
            modelBuilder
                .Entity<Project>()
                .HasOne(p => p.Group)
                .WithMany(g => g.Projects)
                .HasForeignKey(p => p.GroupId);

            // modelBuilder.Entity<Project>()
            //     .Property(p => p.Version)
            //     .IsRowVersion();

            modelBuilder
                .Entity<Project>()
                .HasIndex(p => p.ProjectNumber)
                .IsUnique();

            // Mối quan hệ giữa Group và Employee (GroupLeader)
            modelBuilder
                .Entity<Group>()
                .HasOne(g => g.GroupLeader)
                .WithOne(e => e.Group)
                .HasForeignKey<Group>(g => g.GroupLeaderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Employee>()
                .HasOne(e => e.Group)
                .WithOne(g => g.GroupLeader)
                .OnDelete(DeleteBehavior.Restrict);

            // Mối quan hệ giữa Project và ProjectEmployee
            modelBuilder
                .Entity<ProjectEmployee>()
                .HasKey(pe => new { pe.ProjectId, pe.EmployeeId });

            modelBuilder
                .Entity<ProjectEmployee>()
                .HasOne(pe => pe.Project)
                .WithMany(p => p.ProjectEmployees)
                .HasForeignKey(pe => pe.ProjectId);

            modelBuilder
                .Entity<ProjectEmployee>()
                .HasOne(pe => pe.Employee)
                .WithMany(e => e.ProjectEmployees)
                .HasForeignKey(pe => pe.EmployeeId);
        }

        // public override int SaveChanges()
        // {
        //     foreach (var entry in ChangeTracker.Entries())
        //     {
        //         if (entry.Entity is Employee && entry.State == EntityState.Added)
        //         {
        //             ((Employee)entry.Entity).Version = GetNextTimestamp();
        //         }
        //     }

        //     return base.SaveChanges();
        // }

        // public byte[] GetNextTimestamp()
        // {
        //     return BitConverter.GetBytes(DateTime.UtcNow.Ticks);
        // }
    }
}
