using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Films.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Actor entity
/// </summary>
public class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.ToTable("Actor");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100);

        builder.HasOne(e => e.Sex)
            .WithMany(s => s.Actors)
            .HasForeignKey(e => e.SexId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
