using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Repositories;

/// <summary>
/// User repository implementation using Entity Framework Core
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly FilmsDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(FilmsDbContext context, ILogger<UserRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting all users from database");
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.TypeUser)
                .Include(u => u.Sex)
                .OrderBy(u => u.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users from database");
            throw;
        }
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting user by id: {Id}", id);
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.TypeUser)
                .Include(u => u.Sex)
                .Include(u => u.UserRights)
                    .ThenInclude(ur => ur.Right)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user by id: {Id}", id);
            throw;
        }
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Adding user to database: {UserName}", user.UserName);
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug("User added successfully with id: {Id}", user.Id);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding user to database: {UserName}", user.UserName);
            throw;
        }
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Updating user in database: {Id}", user.Id);
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug("User updated successfully: {Id}", user.Id);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user in database: {Id}", user.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting user from database: {Id}", id);
            var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken);
            if (user != null)
            {
                // Soft delete
                user.IsActive = false;
                user.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogDebug("User soft deleted successfully: {Id}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user from database: {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Checking if user exists: {Id}", id);
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if user exists: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Searching users with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync(cancellationToken);

            return await _context.Users
                .AsNoTracking()
                .Include(u => u.TypeUser)
                .Include(u => u.Sex)
                .Where(u => u.Name.Contains(searchTerm) ||
                           u.UserName.Contains(searchTerm) ||
                           u.Email.Contains(searchTerm) ||
                           (u.Description != null && u.Description.Contains(searchTerm)))
                .OrderBy(u => u.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting user by username: {UserName}", userName);
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.TypeUser)
                .Include(u => u.Sex)
                .Include(u => u.UserRights)
                    .ThenInclude(ur => ur.Right)
                .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user by username: {UserName}", userName);
            throw;
        }
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting user by email: {Email}", email);
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.TypeUser)
                .Include(u => u.Sex)
                .Include(u => u.UserRights)
                    .ThenInclude(ur => ur.Right)
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user by email: {Email}", email);
            throw;
        }
    }

    public async Task<IEnumerable<User>> GetByTypeAsync(int typeUserId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting users by type: {TypeUserId}", typeUserId);
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.TypeUser)
                .Include(u => u.Sex)
                .Where(u => u.TypeUserId == typeUserId)
                .OrderBy(u => u.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting users by type: {TypeUserId}", typeUserId);
            throw;
        }
    }
}