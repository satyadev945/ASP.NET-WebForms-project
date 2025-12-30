namespace Films.Application.DTOs;

public record UserDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime? ModifiedDate { get; init; }
    public bool IsActive { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string? ModifiedBy { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public int? TypeUserId { get; init; }
    public int? SexId { get; init; }
}

public record UserCreateDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public int? TypeUserId { get; init; }
    public int? SexId { get; init; }
}

public record UserUpdateDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public int? TypeUserId { get; init; }
    public int? SexId { get; init; }
}