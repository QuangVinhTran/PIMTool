using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces;

namespace PIMTool.Core.Mappings.DatabaseMapping;

public class GroupModelMapper : IModelMapper
{
    public void Mapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable(nameof(Group));
            
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())");
            
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date");
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("date");

            entity.HasOne(d => d.Leader).WithMany(p => p.Groups)
                .HasForeignKey(d => d.LeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}