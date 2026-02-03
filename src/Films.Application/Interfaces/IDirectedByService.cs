using Films.Domain.Entities;

namespace Films.Application.Interfaces;

public interface IDirectedByService
{
    Task<IEnumerable<DirectedBy>> GetAllDirectorsAsync();
    Task<DirectedBy?> GetDirectorByIdAsync(int id);
    Task<DirectedBy> CreateDirectorAsync(DirectedBy director);
    Task<DirectedBy> UpdateDirectorAsync(DirectedBy director);
    Task<bool> DeleteDirectorAsync(int id);
}