using eShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Infrastructure.Data;

public class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
    {
    }

    public DbSet<CatalogItem> CatalogItems { get; set; } = null!;
    public DbSet<CatalogBrand> CatalogBrands { get; set; } = null!;
    public DbSet<CatalogType> CatalogTypes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CatalogType>(entity =>
        {
            entity.ToTable("CatalogType");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<CatalogBrand>(entity =>
        {
            entity.ToTable("CatalogBrand");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Brand).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<CatalogItem>(entity =>
        {
            entity.ToTable("Catalog");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.PictureFileName).IsRequired();
            entity.Ignore(e => e.PictureFileName);

            entity.HasOne(e => e.CatalogBrand)
                .WithMany()
                .HasForeignKey(e => e.CatalogBrandId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CatalogType)
                .WithMany()
                .HasForeignKey(e => e.CatalogTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
