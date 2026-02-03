using Films.Application.Common.Interfaces;
using Films.Application.Interfaces;
using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Films.Application.Services;

public class UserService : IUserService
{
    private readonly IFilmsDbContext _context;

    public UserService(IFilmsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u => u.TypeUser)
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.TypeUser)
            .Include(u => u.UserRights)
                .ThenInclude(ur => ur.Right)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.TypeUser)
            .Include(u => u.UserRights)
                .ThenInclude(ur => ur.Right)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return false;

        // In a production environment, we would use a secure password hashing library
        // This is a simple hash for demonstration purposes only
        var hashedPassword = HashPassword(password);
        return user.Password == hashedPassword;
    }

    public async Task<User> CreateUserAsync(User user, string password)
    {
        // Hash the password
        user.Password = HashPassword(password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Right>> GetUserRightsAsync(int userId)
    {
        var userRights = await _context.UserRights
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Right)
            .Select(ur => ur.Right)
            .ToListAsync();

        return userRights;
    }

    public async Task AddRightToUserAsync(int userId, int rightId)
    {
        var userRight = new UserRight
        {
            UserId = userId,
            RightId = rightId
        };

        _context.UserRights.Add(userRight);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveRightFromUserAsync(int userId, int rightId)
    {
        var userRight = await _context.UserRights
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RightId == rightId);

        if (userRight != null)
        {
            _context.UserRights.Remove(userRight);
            await _context.SaveChangesAsync();
        }
    }

    private string HashPassword(string password)
    {
        // In a real application, use a proper password hashing library like BCrypt.Net
        // This is a simple example for demonstration
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}