namespace Films.Domain.DTOs;

/// <summary>
/// Actor data transfer object
/// </summary>
public class ActorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? SexId { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }
}

/// <summary>
/// Actor create data transfer object
/// </summary>
public class ActorCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? SexId { get; set; }
    public string? ImageUrl { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

/// <summary>
/// Actor update data transfer object
/// </summary>
public class ActorUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? SexId { get; set; }
    public string? ImageUrl { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
}