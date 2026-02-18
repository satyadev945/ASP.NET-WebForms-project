using Films.Domain.Entities;
using Films.Infrastructure.Data;
using Films.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Films.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for film data access
/// </summary>
public class FilmRepository : IFilmRepository
{
    private readonly FilmsDbContext _context;

    public FilmRepository(FilmsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Film>> GetAllAsync()
    {
        return await _context.Films
            .Include(f => f.ActorFilms)
                .ThenInclude(af => af.Actor)
            .Include(f => f.DirectorFilms)
                .ThenInclude(df => df.Director)
            .ToListAsync();
    }

    public async Task<Film?> GetByIdAsync(int id)
    {
        return await _context.Films
            .Include(f => f.ActorFilms)
                .ThenInclude(af => af.Actor)
            .Include(f => f.DirectorFilms)
                .ThenInclude(df => df.Director)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Film> AddAsync(Film film)
    {
        _context.Films.Add(film);
        await _context.SaveChangesAsync();
        return film;
    }

    public async Task<Film> UpdateAsync(Film film)
    {
        _context.Entry(film).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return film;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var film = await _context.Films.FindAsync(id);
        if (film == null)
            return false;

        _context.Films.Remove(film);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Film>> SearchAsync(string searchTerm)
    {
        return await _context.Films
            .Where(f => f.Title.Contains(searchTerm) || 
                       (f.Description != null && f.Description.Contains(searchTerm)) ||
                       (f.Genre != null && f.Genre.Contains(searchTerm)))
            .Include(f => f.ActorFilms)
                .ThenInclude(af => af.Actor)
            .Include(f => f.DirectorFilms)
                .ThenInclude(df => df.Director)
            .ToListAsync();
    }
}
