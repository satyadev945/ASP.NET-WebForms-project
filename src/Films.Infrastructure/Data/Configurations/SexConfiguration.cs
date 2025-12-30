using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Sex
/// </summary>
public class SexConfiguration : IEntityTypeConfiguration<Sex>
{
    public void Configure(EntityTypeBuilder<Sex> builder)
    {
        builder.ToTable("Sex");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);

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

        // Seed data
        builder.HasData(
            new Sex { Id = 1, Name = "Male", Description = "Male gender", CreatedBy = "System", CreatedDate = DateTime.UtcNow, IsActive = true },
            new Sex { Id = 2, Name = "Female", Description = "Female gender", CreatedBy = "System", CreatedDate = DateTime.UtcNow, IsActive = true }
        );

        // Indexes
        builder.HasIndex(e => e.Name).IsUnique();
        builder.HasIndex(e => e.IsActive);
    }
}