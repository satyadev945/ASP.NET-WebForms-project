using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

public class FilmConfiguration : IEntityTypeConfiguration<Film>
{
    public void Configure(EntityTypeBuilder<Film> builder)
    {
        builder.ToTable("Films");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Genre)
            .HasMaxLength(100);

        builder.Property(x => x.Director)
            .HasMaxLength(255);

        builder.Property(x => x.Country)
            .HasMaxLength(100);

        builder.Property(x => x.Rating)
            .HasColumnType("decimal(3,2)");

        builder.Property(x => x.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.ModifiedDate);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedBy)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.ModifiedBy)
            .HasMaxLength(255);

        // Navigation properties
        builder.HasMany(x => x.FilmActors)
            .WithOne(x => x.Film)
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.FilmDirectors)
            .WithOne(x => x.Film)
            .HasForeignKey(x => x.FilmId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.Year);
    }
}