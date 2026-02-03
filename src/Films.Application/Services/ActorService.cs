using Films.Application.Common.Interfaces;
using Films.Application.Interfaces;
using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Films.Application.Services;

public class ActorService : IActorService
{
    private readonly IFilmsDbContext _context;

    public ActorService(IFilmsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Actor>> GetAllActorsAsync()
    {
        return await _context.Actors
            .Include(a => a.Sex)
            .ToListAsync();
    }

    public async Task<Actor?> GetActorByIdAsync(int id)
    {
        return await _context.Actors
            .Include(a => a.Sex)
            .Include(a => a.FilmReferences)
                .ThenInclude(af => af.Film)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Actor> CreateActorAsync(Actor actor)
    {
        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();
        return actor;
    }

    public async Task<Actor> UpdateActorAsync(Actor actor)
    {
        _context.Entry(actor).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return actor;
    }

    public async Task<bool> DeleteActorAsync(int id)
    {
        var actor = await _context.Actors.FindAsync(id);
        if (actor == null)
            return false;

        _context.Actors.Remove(actor);
        await _context.SaveChangesAsync();
        return true;
    }
}