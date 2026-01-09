namespace Films.Application.DTOs;

/// <summary>
/// Data transfer object for Actor
/// </summary>
public class ActorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? SexId { get; set; }
    public string? SexName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for creating an actor
/// </summary>
public class ActorCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? SexId { get; set; }
}

/// <summary>
/// DTO for updating an actor
/// </summary>
public class ActorUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? SexId { get; set; }
}
