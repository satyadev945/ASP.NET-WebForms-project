using Films.Domain.Entities;

namespace Films.Application.Interfaces;

/// <summary>
/// Service interface for user operations
/// </summary>
public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User> CreateUserAsync(User user, string password);
    Task<User> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int id);
    Task<bool> ValidateUserAsync(string username, string password);
}
