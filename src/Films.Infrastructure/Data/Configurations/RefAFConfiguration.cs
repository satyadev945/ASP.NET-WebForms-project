using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

public class RefAFConfiguration : IEntityTypeConfiguration<RefAF>
{
    public void Configure(EntityTypeBuilder<RefAF> builder)
    {
        builder.ToTable("RefAF");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(e => e.Actor)
            .WithMany(a => a.RefAFs)
            .HasForeignKey(e => e.ActorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Film)
            .WithMany(f => f.RefAFs)
            .HasForeignKey(e => e.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
