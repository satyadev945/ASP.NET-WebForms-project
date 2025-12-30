using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Right
/// </summary>
public class RightConfiguration : IEntityTypeConfiguration<Right>
{
    public void Configure(EntityTypeBuilder<Right> builder)
    {
        builder.ToTable("Rights");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasMaxLength(255);

        builder.Property(e => e.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(255);

        // Configure relationships
        builder.HasMany(e => e.UserRights)
            .WithOne(e => e.Right)
            .HasForeignKey(e => e.RightId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed data
        builder.HasData(
            new Right { Id = 1, Name = "Read", Description = "Read access", CreatedBy = "System", CreatedDate = DateTime.UtcNow, IsActive = true },
            new Right { Id = 2, Name = "Write", Description = "Write access", CreatedBy = "System", CreatedDate = DateTime.UtcNow, IsActive = true },
            new Right { Id = 3, Name = "Delete", Description = "Delete access", CreatedBy = "System", CreatedDate = DateTime.UtcNow, IsActive = true },
            new Right { Id = 4, Name = "Admin", Description = "Administrative access", CreatedBy = "System", CreatedDate = DateTime.UtcNow, IsActive = true }
        );

        // Indexes
        builder.HasIndex(e => e.Name).IsUnique();
        builder.HasIndex(e => e.IsActive);
    }
}