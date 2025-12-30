using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for RefDAF (Director-Film relationship)
/// </summary>
public class RefDAFConfiguration : IEntityTypeConfiguration<RefDAF>
{
    public void Configure(EntityTypeBuilder<RefDAF> builder)
    {
        builder.ToTable("RefDAF");

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
        builder.HasOne(e => e.Director)
            .WithMany(e => e.RefDAFs)
            .HasForeignKey(e => e.DirectorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Film)
            .WithMany(e => e.RefDAFs)
            .HasForeignKey(e => e.FilmId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(e => new { e.DirectorId, e.FilmId }).IsUnique();
        builder.HasIndex(e => e.DirectorId);
        builder.HasIndex(e => e.FilmId);
        builder.HasIndex(e => e.IsActive);
    }
}