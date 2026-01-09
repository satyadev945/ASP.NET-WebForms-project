using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for RefAF entity
/// </summary>
public class RefAFConfiguration : IEntityTypeConfiguration<RefAF>
{
    public void Configure(EntityTypeBuilder<RefAF> builder)
    {
        builder.ToTable("RefAF");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.CreatedDate)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(e => e.Actor)
            .WithMany(e => e.RefAFs)
            .HasForeignKey(e => e.ActorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Film)
            .WithMany(e => e.RefAFs)
            .HasForeignKey(e => e.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
