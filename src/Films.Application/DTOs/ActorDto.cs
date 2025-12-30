namespace Films.Application.DTOs;

public record ActorDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime? ModifiedDate { get; init; }
    public bool IsActive { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string? ModifiedBy { get; init; }
}

public record ActorCreateDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}

public record ActorUpdateDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}