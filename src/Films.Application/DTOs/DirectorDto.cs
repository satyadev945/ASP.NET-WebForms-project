namespace Films.Application.DTOs;

/// <summary>
/// Data transfer object for Director
/// </summary>
public class DirectorDto
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
/// DTO for creating a director
/// </summary>
public class DirectorCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? SexId { get; set; }
}

/// <summary>
/// DTO for updating a director
/// </summary>
public class DirectorUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? SexId { get; set; }
}
