using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Data;

public class FilmsDbContext : DbContext
{
    public FilmsDbContext(DbContextOptions<FilmsDbContext> options) : base(options)
    {
    }

    public DbSet<Actor> Actors { get; set; }
    public DbSet<Film> Films { get; set; }
    public DbSet<DirectedBy> Directors { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<TypeUser> TypeUsers { get; set; }
    public DbSet<Sex> Sexes { get; set; }
    public DbSet<Right> Rights { get; set; }
    public DbSet<RefAF> FilmActors { get; set; }
    public DbSet<RefDAF> FilmDirectors { get; set; }
    public DbSet<UserRight> UserRights { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FilmsDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }
    }
}