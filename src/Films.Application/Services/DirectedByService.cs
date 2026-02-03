using Films.Application.Common.Interfaces;
using Films.Application.Interfaces;
using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Films.Application.Services;

public class DirectedByService : IDirectedByService
{
    private readonly IFilmsDbContext _context;

    public DirectedByService(IFilmsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DirectedBy>> GetAllDirectorsAsync()
    {
        return await _context.DirectedBys.ToListAsync();
    }

    public async Task<DirectedBy?> GetDirectorByIdAsync(int id)
    {
        return await _context.DirectedBys
            .Include(d => d.FilmReferences)
                .ThenInclude(df => df.Film)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<DirectedBy> CreateDirectorAsync(DirectedBy director)
    {
        _context.DirectedBys.Add(director);
        await _context.SaveChangesAsync();
        return director;
    }

    public async Task<DirectedBy> UpdateDirectorAsync(DirectedBy director)
    {
        _context.Entry(director).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return director;
    }

    public async Task<bool> DeleteDirectorAsync(int id)
    {
        var director = await _context.DirectedBys.FindAsync(id);
        if (director == null)
            return false;

        _context.DirectedBys.Remove(director);
        await _context.SaveChangesAsync();
        return true;
    }
}