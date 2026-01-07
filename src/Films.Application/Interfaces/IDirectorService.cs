using Films.Application.DTOs;

namespace Films.Application.Interfaces;

public interface IDirectorService
{
    Task<IEnumerable<DirectorDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DirectorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<DirectorDto> CreateAsync(DirectorCreateDto directorCreateDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, DirectorUpdateDto directorUpdateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DirectorDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
