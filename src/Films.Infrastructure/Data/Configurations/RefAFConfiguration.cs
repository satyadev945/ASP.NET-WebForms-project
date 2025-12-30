using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for RefAF (Actor-Film relationship)
/// </summary>
public class RefAFConfiguration : IEntityTypeConfiguration<RefAF>
{
    public void Configure(EntityTypeBuilder<RefAF> builder)
    {
        builder.ToTable("RefAF");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Role)
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
        builder.HasOne(e => e.Actor)
            .WithMany(e => e.RefAFs)
            .HasForeignKey(e => e.ActorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Film)
            .WithMany(e => e.RefAFs)
            .HasForeignKey(e => e.FilmId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(e => new { e.ActorId, e.FilmId }).IsUnique();
        builder.HasIndex(e => e.ActorId);
        builder.HasIndex(e => e.FilmId);
        builder.HasIndex(e => e.IsActive);
    }
}