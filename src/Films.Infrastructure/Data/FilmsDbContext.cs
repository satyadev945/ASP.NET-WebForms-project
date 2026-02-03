using Films.Application.Common.Interfaces;
using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Data;

public class FilmsDbContext : DbContext, IFilmsDbContext
{
    public FilmsDbContext(DbContextOptions<FilmsDbContext> options) : base(options)
    {
    }

    public DbSet<Actor> Actors { get; set; } = null!;
    public DbSet<DirectedBy> DirectedBys { get; set; } = null!;
    public DbSet<Film> Films { get; set; } = null!;
    public DbSet<RefAF> RefAFs { get; set; } = null!;
    public DbSet<RefDAF> RefDAFs { get; set; } = null!;
    public DbSet<Right> Rights { get; set; } = null!;
    public DbSet<Sex> Sexes { get; set; } = null!;
    public DbSet<TypeUser> TypeUsers { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserRight> UserRights { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Actor>(entity =>
        {
            entity.ToTable("Actor");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();

            entity.HasOne(e => e.Sex)
                .WithMany(e => e.Actors)
                .HasForeignKey(e => e.SexId);
        });

        modelBuilder.Entity<DirectedBy>(entity =>
        {
            entity.ToTable("DirectedBy");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Film>(entity =>
        {
            entity.ToTable("Film");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<RefAF>(entity =>
        {
            entity.ToTable("RefAF");
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Actor)
                .WithMany(e => e.FilmReferences)
                .HasForeignKey(e => e.ActorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Film)
                .WithMany(e => e.ActorReferences)
                .HasForeignKey(e => e.FilmId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RefDAF>(entity =>
        {
            entity.ToTable("RefDAF");
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.DirectedBy)
                .WithMany(e => e.FilmReferences)
                .HasForeignKey(e => e.DirectedById)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Film)
                .WithMany(e => e.DirectorReferences)
                .HasForeignKey(e => e.FilmId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Right>(entity =>
        {
            entity.ToTable("Right");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Sex>(entity =>
        {
            entity.ToTable("Sex");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<TypeUser>(entity =>
        {
            entity.ToTable("TypeUser");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Password).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(100).IsRequired();

            entity.HasOne(e => e.TypeUser)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.TypeUserId);
        });

        modelBuilder.Entity<UserRight>(entity =>
        {
            entity.ToTable("UserRight");
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.User)
                .WithMany(e => e.UserRights)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Right)
                .WithMany(e => e.UserRights)
                .HasForeignKey(e => e.RightId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}