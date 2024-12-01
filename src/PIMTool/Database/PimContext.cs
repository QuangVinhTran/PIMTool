using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PIMTool.Core.Domain.Entities;

namespace PIMTool.Database
{
    public partial class PimContext : DbContext
    {
        public PimContext()
        {
        }

        public PimContext(DbContextOptions<PimContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<ProjectEmployee> ProjectEmployees { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
            var strConn = config["ConnectionStrings:PimTool"];

            return strConn;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1 - 1 ProjGroupect and Employee
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Visa).IsUnique();
                entity.HasOne(e => e.Group)
                .WithOne(g => g.GroupLeader)
                .HasForeignKey<Group>(g => g.GroupLeaderId);
            });
                

            // M - M Project and Employee
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Employees)
                .WithMany(e => e.Projects)
                .UsingEntity<ProjectEmployee>(
                    l => l.HasOne(e => e.Employee).WithMany(e => e.ProjectEmployees).HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Restrict),
                    r => r.HasOne(e => e.Project).WithMany(e => e.ProjectEmployees).HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Restrict)
            );


            // 1 - M Project and Group
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasIndex(p => p.ProjectNumber).IsUnique();
                entity.HasOne(p => p.Group)
                .WithMany(g => g.Projects)
                .HasForeignKey(p => p.GroupId)
                .IsRequired();
            });
                
        }
    }
}
