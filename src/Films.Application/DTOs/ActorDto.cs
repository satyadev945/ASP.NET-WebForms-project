namespace Films.Application.DTOs;

/// <summary>
/// Data Transfer Object for Actor entity
/// </summary>
public class ActorDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? SexId { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Biography { get; set; }
}

/// <summary>
/// DTO for creating a new actor
/// </summary>
public class CreateActorDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? SexId { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Biography { get; set; }
}

/// <summary>
/// DTO for updating an existing actor
/// </summary>
public class UpdateActorDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? SexId { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Biography { get; set; }
}
