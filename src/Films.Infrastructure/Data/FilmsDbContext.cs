using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Data;

/// <summary>
/// Database context for the Films application - PostgreSQL compatible
/// </summary>
public class FilmsDbContext : DbContext
{
    public FilmsDbContext(DbContextOptions<FilmsDbContext> options) : base(options)
    {
    }

    public DbSet<Film> Films { get; set; } = null!;
    public DbSet<Actor> Actors { get; set; } = null!;
    public DbSet<Director> Directors { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Sex> Sexes { get; set; } = null!;
    public DbSet<TypeUser> TypeUsers { get; set; } = null!;
    public DbSet<Right> Rights { get; set; } = null!;
    public DbSet<UserRight> UserRights { get; set; } = null!;
    public DbSet<RefAF> ActorFilms { get; set; } = null!;
    public DbSet<RefDAF> DirectorFilms { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set default schema to public for PostgreSQL
        modelBuilder.HasDefaultSchema("public");

        // Film configuration
        modelBuilder.Entity<Film>(entity =>
        {
            entity.ToTable("films");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200).HasColumnName("title");
            entity.Property(e => e.Description).HasMaxLength(2000).HasColumnName("description");
            entity.Property(e => e.Genre).HasMaxLength(100).HasColumnName("genre");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date").HasColumnType("timestamp without time zone");
            entity.Property(e => e.ModifiedDate).HasColumnName("modified_date").HasColumnType("timestamp without time zone");
        });

        // Actor configuration
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.ToTable("actors");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100).HasColumnName("first_name");
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100).HasColumnName("last_name");
            entity.Property(e => e.Biography).HasMaxLength(2000).HasColumnName("biography");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date").HasColumnType("timestamp without time zone");
            entity.Property(e => e.SexId).HasColumnName("sex_id");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date").HasColumnType("timestamp without time zone");
            entity.Property(e => e.ModifiedDate).HasColumnName("modified_date").HasColumnType("timestamp without time zone");
            
            entity.HasOne(e => e.Sex)
                .WithMany(s => s.Actors)
                .HasForeignKey(e => e.SexId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Director configuration
        modelBuilder.Entity<Director>(entity =>
        {
            entity.ToTable("directors");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100).HasColumnName("first_name");
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100).HasColumnName("last_name");
            entity.Property(e => e.Biography).HasMaxLength(2000).HasColumnName("biography");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date").HasColumnType("timestamp without time zone");
            entity.Property(e => e.SexId).HasColumnName("sex_id");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date").HasColumnType("timestamp without time zone");
            entity.Property(e => e.ModifiedDate).HasColumnName("modified_date").HasColumnType("timestamp without time zone");
            
            entity.HasOne(e => e.Sex)
                .WithMany(s => s.Directors)
                .HasForeignKey(e => e.SexId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100).HasColumnName("username");
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200).HasColumnName("email");
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500).HasColumnName("password_hash");
            entity.Property(e => e.FirstName).HasMaxLength(100).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasMaxLength(100).HasColumnName("last_name");
            entity.Property(e => e.TypeUserId).HasColumnName("type_user_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date").HasColumnType("timestamp without time zone");
            entity.Property(e => e.ModifiedDate).HasColumnName("modified_date").HasColumnType("timestamp without time zone");
            entity.Property(e => e.LastLoginDate).HasColumnName("last_login_date").HasColumnType("timestamp without time zone");
            
            entity.HasOne(e => e.TypeUser)
                .WithMany(t => t.Users)
                .HasForeignKey(e => e.TypeUserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Sex configuration
        modelBuilder.Entity<Sex>(entity =>
        {
            entity.ToTable("sex");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50).HasColumnName("name");
            entity.Property(e => e.Description).HasMaxLength(200).HasColumnName("description");
        });

        // TypeUser configuration
        modelBuilder.Entity<TypeUser>(entity =>
        {
            entity.ToTable("type_user");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.Description).HasMaxLength(200).HasColumnName("description");
        });

        // Right configuration
        modelBuilder.Entity<Right>(entity =>
        {
            entity.ToTable("rights");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.Description).HasMaxLength(200).HasColumnName("description");
            entity.Property(e => e.Code).HasMaxLength(50).HasColumnName("code");
        });

        // UserRight configuration
        modelBuilder.Entity<UserRight>(entity =>
        {
            entity.ToTable("user_rights");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GrantedDate).HasColumnName("granted_date").HasColumnType("timestamp without time zone");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.RightId).HasColumnName("right_id");
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.UserRights)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Right)
                .WithMany(r => r.UserRights)
                .HasForeignKey(e => e.RightId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // RefAF (Actor-Film) configuration
        modelBuilder.Entity<RefAF>(entity =>
        {
            entity.ToTable("ref_af");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActorId).HasColumnName("actor_id");
            entity.Property(e => e.FilmId).HasColumnName("film_id");
            entity.Property(e => e.Role).HasMaxLength(100).HasColumnName("role");
            
            entity.HasOne(e => e.Actor)
                .WithMany(a => a.ActorFilms)
                .HasForeignKey(e => e.ActorId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Film)
                .WithMany(f => f.ActorFilms)
                .HasForeignKey(e => e.FilmId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // RefDAF (Director-Film) configuration
        modelBuilder.Entity<RefDAF>(entity =>
        {
            entity.ToTable("ref_daf");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DirectorId).HasColumnName("director_id");
            entity.Property(e => e.FilmId).HasColumnName("film_id");
            
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
