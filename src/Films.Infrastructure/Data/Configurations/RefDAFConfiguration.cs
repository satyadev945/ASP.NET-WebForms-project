using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for RefDAF entity
/// </summary>
public class RefDAFConfiguration : IEntityTypeConfiguration<RefDAF>
{
    public void Configure(EntityTypeBuilder<RefDAF> builder)
    {
        builder.ToTable("RefDAF");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.CreatedDate)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(e => e.Director)
            .WithMany(e => e.RefDAFs)
            .HasForeignKey(e => e.DirectorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Film)
            .WithMany(e => e.RefDAFs)
            .HasForeignKey(e => e.FilmId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
