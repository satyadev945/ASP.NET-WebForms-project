namespace Films.Application.DTOs;

public record FilmDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime? ModifiedDate { get; init; }
    public bool IsActive { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string? ModifiedBy { get; init; }
    public string? Genre { get; init; }
    public int? Year { get; init; }
    public string? Director { get; init; }
    public string? Country { get; init; }
    public int? Duration { get; init; }
    public decimal? Rating { get; init; }
}

public record FilmCreateDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Genre { get; init; }
    public int? Year { get; init; }
    public string? Director { get; init; }
    public string? Country { get; init; }
    public int? Duration { get; init; }
    public decimal? Rating { get; init; }
}

public record FilmUpdateDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Genre { get; init; }
    public int? Year { get; init; }
    public string? Director { get; init; }
    public string? Country { get; init; }
    public int? Duration { get; init; }
    public decimal? Rating { get; init; }
}