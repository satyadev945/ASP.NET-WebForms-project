using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Films.Application.Common.Interfaces;

public interface IFilmsDbContext
{
    DbSet<Actor> Actors { get; }
    DbSet<DirectedBy> DirectedBys { get; }
    DbSet<Film> Films { get; }
    DbSet<RefAF> RefAFs { get; }
    DbSet<RefDAF> RefDAFs { get; }
    DbSet<Right> Rights { get; }
    DbSet<Sex> Sexes { get; }
    DbSet<TypeUser> TypeUsers { get; }
    DbSet<User> Users { get; }
    DbSet<UserRight> UserRights { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}