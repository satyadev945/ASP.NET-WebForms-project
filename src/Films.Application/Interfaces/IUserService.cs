using Films.Domain.Entities;

namespace Films.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<bool> ValidateUserCredentialsAsync(string username, string password);
    Task<User> CreateUserAsync(User user, string password);
    Task<User> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int id);
    Task<IEnumerable<Right>> GetUserRightsAsync(int userId);
    Task AddRightToUserAsync(int userId, int rightId);
    Task RemoveRightFromUserAsync(int userId, int rightId);
}