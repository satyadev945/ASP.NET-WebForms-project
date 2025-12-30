namespace Films.Domain.DTOs;

/// <summary>
/// Film data transfer object
/// </summary>
public class FilmDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Year { get; set; }
    public string? Genre { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }
}

/// <summary>
/// Film create data transfer object
/// </summary>
public class FilmCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Year { get; set; }
    public string? Genre { get; set; }
    public string? ImageUrl { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

/// <summary>
/// Film update data transfer object
/// </summary>
public class FilmUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Year { get; set; }
    public string? Genre { get; set; }
    public string? ImageUrl { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
}