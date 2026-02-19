using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Data;

/// <summary>
/// Database context for the Films application
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
    public DbSet<UserRight> UserRights { get; set; }
    public DbSet<RefAF> ActorFilms { get; set; }
    public DbSet<RefDAF> DirectorFilms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Film entity
        modelBuilder.Entity<Film>(entity =>
        {
            entity.ToTable("Films");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Genre).HasMaxLength(100);
        });

        // Configure Actor entity
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.ToTable("Actors");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Sex)
                .WithMany(s => s.Actors)
                .HasForeignKey(e => e.SexId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Director entity
        modelBuilder.Entity<Director>(entity =>
        {
            entity.ToTable("Directors");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Sex)
                .WithMany(s => s.Directors)
                .HasForeignKey(e => e.SexId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Sex entity
        modelBuilder.Entity<Sex>(entity =>
        {
            entity.ToTable("Sex");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
        });

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.HasOne(e => e.Sex)
                .WithMany(s => s.Users)
                .HasForeignKey(e => e.SexId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.TypeUser)
                .WithMany(t => t.Users)
                .HasForeignKey(e => e.TypeUserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure TypeUser entity
        modelBuilder.Entity<TypeUser>(entity =>
        {
            entity.ToTable("TypeUser");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });

        // Configure Right entity
        modelBuilder.Entity<Right>(entity =>
        {
            entity.ToTable("Rights");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });

        // Configure UserRight entity
        modelBuilder.Entity<UserRight>(entity =>
        {
            entity.ToTable("UserRights");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithMany(u => u.UserRights)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Right)
                .WithMany(r => r.UserRights)
                .HasForeignKey(e => e.RightId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure RefAF entity (Actor-Film relationship)
        modelBuilder.Entity<RefAF>(entity =>
        {
            entity.ToTable("RefAF");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Actor)
                .WithMany(a => a.ActorFilms)
                .HasForeignKey(e => e.ActorId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Film)
                .WithMany(f => f.ActorFilms)
                .HasForeignKey(e => e.FilmId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure RefDAF entity (Director-Film relationship)
        modelBuilder.Entity<RefDAF>(entity =>
        {
            entity.ToTable("RefDAF");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Director)
                .WithMany(d => d.DirectorFilms)
                .HasForeignKey(e => e.DirectorId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Film)
                .WithMany(f => f.DirectorFilms)
                .HasForeignKey(e => e.FilmId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
