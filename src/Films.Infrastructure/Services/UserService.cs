using Films.Application.Interfaces;
using Films.Domain.Entities;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Films.Infrastructure.Services;

/// <summary>
/// Service implementation for user operations
/// </summary>
public class UserService : IUserService
{
    private readonly FilmsDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(FilmsDbContext context, ILogger<UserService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all users");
            return await _context.Users
                .Include(u => u.Sex)
                .Include(u => u.TypeUser)
                .Include(u => u.UserRights)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            throw;
        }
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving user with ID: {UserId}", id);
            return await _context.Users
                .Include(u => u.Sex)
                .Include(u => u.TypeUser)
                .Include(u => u.UserRights)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID: {UserId}", id);
            throw;
        }
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        try
        {
            _logger.LogInformation("Retrieving user with username: {Username}", username);
            return await _context.Users
                .Include(u => u.Sex)
                .Include(u => u.TypeUser)
                .Include(u => u.UserRights)
                .FirstOrDefaultAsync(u => u.Username == username);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with username: {Username}", username);
            throw;
        }
    }

    public async Task<User> CreateUserAsync(User user, string password)
    {
        try
        {
            _logger.LogInformation("Creating new user: {Username}", user.Username);
            user.PasswordHash = HashPassword(password);
            user.CreatedDate = DateTime.UtcNow;
            user.IsActive = true;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User created successfully with ID: {UserId}", user.Id);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {Username}", user.Username);
            throw;
        }
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        try
        {
            _logger.LogInformation("Updating user with ID: {UserId}", user.Id);
            user.ModifiedDate = DateTime.UtcNow;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User updated successfully with ID: {UserId}", user.Id);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID: {UserId}", user.Id);
            throw;
        }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", id);
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID: {UserId} not found", id);
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User deleted successfully with ID: {UserId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID: {UserId}", id);
            throw;
        }
    }

    public async Task<bool> ValidateUserAsync(string username, string password)
    {
        try
        {
            _logger.LogInformation("Validating user: {Username}", username);
            var user = await GetUserByUsernameAsync(username);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("User validation failed for: {Username}", username);
                return false;
            }

            var hashedPassword = HashPassword(password);
            var isValid = user.PasswordHash == hashedPassword;
            _logger.LogInformation("User validation result for {Username}: {IsValid}", username, isValid);
            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating user: {Username}", username);
            throw;
        }
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}
