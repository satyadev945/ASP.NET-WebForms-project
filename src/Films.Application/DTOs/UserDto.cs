namespace Films.Application.DTOs;

/// <summary>
/// Data Transfer Object for User entity
/// </summary>
public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? TypeUserId { get; set; }
    public string? TypeUserName { get; set; }
    public string? Phone { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for creating a new user
/// </summary>
public class UserCreateDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? TypeUserId { get; set; }
    public string? Phone { get; set; }
    public string? SecretQuestion { get; set; }
    public string? SecretAnswer { get; set; }
}

/// <summary>
/// DTO for updating an existing user
/// </summary>
public class UserUpdateDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? TypeUserId { get; set; }
    public string? Phone { get; set; }
    public string? SecretQuestion { get; set; }
    public string? SecretAnswer { get; set; }
}

/// <summary>
/// DTO for user login
/// </summary>
public class UserLoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
