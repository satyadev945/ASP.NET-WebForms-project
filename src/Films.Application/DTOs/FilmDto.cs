namespace Films.Application.DTOs;

/// <summary>
/// Data transfer object for Film
/// </summary>
public class FilmDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? Year { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for creating a film
/// </summary>
public class FilmCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? Year { get; set; }
}

/// <summary>
/// DTO for updating a film
/// </summary>
public class FilmUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? Year { get; set; }
}
