using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for UserRight (User-Right relationship)
/// </summary>
public class UserRightConfiguration : IEntityTypeConfiguration<UserRight>
{
    public void Configure(EntityTypeBuilder<UserRight> builder)
    {
        builder.ToTable("UserRights");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

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
        builder.HasOne(e => e.User)
            .WithMany(e => e.UserRights)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Right)
            .WithMany(e => e.UserRights)
            .HasForeignKey(e => e.RightId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(e => new { e.UserId, e.RightId }).IsUnique();
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.RightId);
        builder.HasIndex(e => e.IsActive);
    }
}