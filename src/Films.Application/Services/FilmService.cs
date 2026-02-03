using Films.Application.Common.Interfaces;
using Films.Application.Interfaces;
using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Films.Application.Services;

public class FilmService : IFilmService
{
    private readonly IFilmsDbContext _context;

    public FilmService(IFilmsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Film>> GetAllFilmsAsync()
    {
        return await _context.Films.ToListAsync();
    }

    public async Task<Film?> GetFilmByIdAsync(int id)
    {
        return await _context.Films
            .Include(f => f.ActorReferences)
                .ThenInclude(af => af.Actor)
                    .ThenInclude(a => a.Sex)
            .Include(f => f.DirectorReferences)
                .ThenInclude(df => df.DirectedBy)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Film> CreateFilmAsync(Film film)
    {
        _context.Films.Add(film);
        await _context.SaveChangesAsync();
        return film;
    }

    public async Task<Film> UpdateFilmAsync(Film film)
    {
        _context.Entry(film).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return film;
    }

    public async Task<bool> DeleteFilmAsync(int id)
    {
        var film = await _context.Films.FindAsync(id);
        if (film == null)
            return false;

        _context.Films.Remove(film);
        await _context.SaveChangesAsync();
        return true;
    }
}