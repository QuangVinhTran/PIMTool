using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces;

namespace PIMTool.Core.Mappings.DatabaseMapping;

public class PIMUserModelMapper : IModelMapper
{
    public void Mapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PIMUser>(entity =>
        {
            entity.ToTable(nameof(PIMUser));
            
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())");
            
            entity.Property(e => e.BirthDate)
                .HasColumnType("date");
            
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date");
            
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            
            entity.Property(e => e.FirstName)
                .HasMaxLength(50);
            
            entity.Property(e => e.LastName)
                .HasMaxLength(50);
            
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('Employee')");
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("date");
        });
    }
}