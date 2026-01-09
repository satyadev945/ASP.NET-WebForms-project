using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Director entity
/// </summary>
public class DirectorConfiguration : IEntityTypeConfiguration<DirectedBy>
{
    public void Configure(EntityTypeBuilder<DirectedBy> builder)
    {
        builder.ToTable("DirectedBy");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Surname)
            .HasMaxLength(100);

        builder.Property(e => e.Bio)
            .HasMaxLength(2000);

        builder.Property(e => e.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasDefaultValue("System");

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100);

        builder.HasOne(e => e.Sex)
            .WithMany(e => e.Directors)
            .HasForeignKey(e => e.SexId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(e => e.DirectorFilms)
            .WithOne(e => e.Director)
            .HasForeignKey(e => e.DirectorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
