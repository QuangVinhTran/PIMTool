using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces;

namespace PIMTool.Core.Mappings.DatabaseMapping;

public class EmployeeModelMapper : IModelMapper
{
    public void Mapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable(nameof(Employee));
            
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC0788608187");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())");
            
            entity.Property(e => e.BirthDate)
                .HasColumnType("date");
            
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date");
            
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            
            entity.Property(e => e.UpdatedAt).HasColumnType("date");
            
            entity.Property(e => e.Visa)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
        });
    }
}