using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for TypeUser
/// </summary>
public class TypeUserConfiguration : IEntityTypeConfiguration<TypeUser>
{
    public void Configure(EntityTypeBuilder<TypeUser> builder)
    {
        builder.ToTable("TypeUser");

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

        // Seed data
        builder.HasData(
            new TypeUser { Id = 1, Name = "Administrator", Description = "System administrator", CreatedBy = "System", CreatedDate = DateTime.UtcNow, IsActive = true },
            new TypeUser { Id = 2, Name = "User", Description = "Regular user", CreatedBy = "System", CreatedDate = DateTime.UtcNow, IsActive = true }
        );

        // Indexes
        builder.HasIndex(e => e.Name).IsUnique();
        builder.HasIndex(e => e.IsActive);
    }
}