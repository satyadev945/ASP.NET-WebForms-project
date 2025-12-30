using Films.Domain.Entities;
using Films.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Data;

/// <summary>
/// Entity Framework Core database context for Films application
/// </summary>
public class FilmsDbContext : DbContext
{
    public FilmsDbContext(DbContextOptions<FilmsDbContext> options) : base(options)
    {
    }

    public DbSet<Film> Films { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<Sex> Sexes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<TypeUser> TypeUsers { get; set; }
    public DbSet<Right> Rights { get; set; }
    public DbSet<RefAF> RefAFs { get; set; }
    public DbSet<RefDAF> RefDAFs { get; set; }
    public DbSet<UserRight> UserRights { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply entity configurations
        modelBuilder.ApplyConfiguration(new FilmConfiguration());
        modelBuilder.ApplyConfiguration(new ActorConfiguration());
        modelBuilder.ApplyConfiguration(new DirectorConfiguration());
        modelBuilder.ApplyConfiguration(new SexConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TypeUserConfiguration());
        modelBuilder.ApplyConfiguration(new RightConfiguration());
        modelBuilder.ApplyConfiguration(new RefAFConfiguration());
        modelBuilder.ApplyConfiguration(new RefDAFConfiguration());
        modelBuilder.ApplyConfiguration(new UserRightConfiguration());

        // Global query filters for soft delete
        modelBuilder.Entity<Film>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Actor>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Director>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Sex>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<User>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<TypeUser>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Right>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<RefAF>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<RefDAF>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<UserRight>().HasQueryFilter(e => e.IsActive);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Set timestamps
        var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Property("CreatedDate") != null)
                    entry.Property("CreatedDate").CurrentValue = DateTime.UtcNow;
                if (entry.Property("IsActive") != null)
                    entry.Property("IsActive").CurrentValue = true;
            }
            else if (entry.State == EntityState.Modified)
            {
                if (entry.Property("ModifiedDate") != null)
                    entry.Property("ModifiedDate").CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}