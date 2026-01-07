using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for User entity
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Username)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Password)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Phone)
            .HasMaxLength(50);

        builder.Property(e => e.SecretQuestion)
            .HasMaxLength(500);

        builder.Property(e => e.SecretAnswer)
            .HasMaxLength(500);

        builder.Property(e => e.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100);

        builder.HasOne(e => e.TypeUser)
            .WithMany(t => t.Users)
            .HasForeignKey(e => e.TypeUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.Username).IsUnique();
    }
}
