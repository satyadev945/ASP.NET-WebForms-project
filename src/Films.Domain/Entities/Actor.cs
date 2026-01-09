namespace Films.Domain.Entities;

/// <summary>
/// Represents an actor entity in the domain
/// </summary>
public class Actor
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Surname { get; set; }

    public int? SexId { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Bio { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string? ModifiedBy { get; set; }

    public Sex? Sex { get; set; }

    public ICollection<RefAF> ActorFilms { get; set; } = new List<RefAF>();
}
