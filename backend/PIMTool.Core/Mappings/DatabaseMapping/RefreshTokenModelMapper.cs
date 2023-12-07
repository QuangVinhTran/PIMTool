using Microsoft.EntityFrameworkCore;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces;

namespace PIMTool.Core.Mappings.DatabaseMapping;

public class RefreshTokenModelMapper : IModelMapper
{
    public void Mapping(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable(nameof(RefreshToken));
            
            entity.HasKey(e => e.Token);
            
            entity.Property(e => e.Token)
                .HasDefaultValueSql("(newid())");
            
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime");
            
            entity.Property(e => e.IPAddress)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName(nameof(RefreshToken.IPAddress));
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime");
            
            entity.Property(e => e.ValidUntil)
                .HasColumnType("datetime");

            entity.Ignore(e => e.Id);
            entity.Ignore(e => e.IsDeleted);

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId);
        });
    }
}