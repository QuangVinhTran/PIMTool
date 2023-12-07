using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces;

namespace PIMTool.Core.Mappings.DatabaseMapping;

public class ProjectModelMapper : IModelMapper
{
    public void Mapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable(nameof(Project));
            
            entity.HasKey(e => e.Id).HasName("PK__Project__3214EC0770E307B9");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())");
            
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date");
            
            entity.Property(e => e.Customer)
                .HasMaxLength(50)
                .IsUnicode(false);
            
            entity.Property(e => e.EndDate)
                .HasColumnType("date");
            
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            
            entity.Property(e => e.StartDate)
                .HasColumnType("date");
            
            entity.Property(e => e.Status)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("date");

            entity.HasOne(d => d.Group).WithMany(p => p.Projects)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientNoAction)
                .HasConstraintName("FK__Project__GroupId__53D770D6");

            entity.HasMany(d => d.Employees).WithMany(p => p.Projects)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectEmployee",
                    r => r.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Project_E__Emplo__5D60DB10"),
                    l => l.HasOne<Project>().WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Project_E__Proje__5F492382"),
                    j =>
                    {
                        j.HasKey("ProjectId", "EmployeeId");
                        j.ToTable("Project_Employee");
                    });

            // entity.HasIndex(e => e.ProjectNumber)
            //     .IsUnique();
            //
            // entity.HasIndex(e => e.Name);
            //
            // entity.HasIndex(e => e.Customer);
            //
            // entity.HasIndex(e => e.StartDate);
            //
            // entity.HasIndex(e => e.EndDate);
            //
            // entity.HasIndex(e => e.Status);
            //
            // entity.HasIndex(e => e.GroupId);
            //
            // entity.HasIndex(e => e.IsDeleted);

        });
    }
}