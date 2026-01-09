namespace Films.Domain.Entities;

/// <summary>
/// Represents a many-to-many relationship between Actors and Films
/// </summary>
public class RefAF
{
    public int Id { get; set; }

    public int ActorId { get; set; }

    public int FilmId { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }

    public Actor? Actor { get; set; }

    public Film? Film { get; set; }
}
