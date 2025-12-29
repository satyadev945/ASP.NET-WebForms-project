using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

public class RefDAFConfiguration : IEntityTypeConfiguration<RefDAF>
{
    public void Configure(EntityTypeBuilder<RefDAF> builder)
    {
        builder.ToTable("RefDAF");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(e => e.Director)
            .WithMany(d => d.RefDAFs)
            .HasForeignKey(e => e.DirectorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Film)
            .WithMany(f => f.RefDAFs)
            .HasForeignKey(e => e.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
